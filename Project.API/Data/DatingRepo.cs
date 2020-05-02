using Microsoft.EntityFrameworkCore;
using Project.API.Dtos;
using Project.API.Helpers;
using Project.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Data
{
    public class DatingRepo : IDatingRepo
    {
        private readonly MyDbContext _dbContext;

        public DatingRepo(MyDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public void Add<T>(T entity) where T : class
        {
            _dbContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _dbContext.Remove(entity);
        }

        public async Task<Likes> GetLikes(int userid, int likeuserid)
        {
            var result = await _dbContext.Likes.FirstOrDefaultAsync(q => q.LikeByUserID == userid
            && q.LikedUserID == likeuserid);

            return result;
        }

        public async Task<Messages> GetMessage(int messageid)
        {
            return await _dbContext.Messages.FirstOrDefaultAsync(q => q.MessageID == messageid);
        }

        public async Task<PageList<Messages>> GetMessagesForUser(MessageParamsDto query)
        {
            var messages = _dbContext.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Receiver).ThenInclude(p => p.Photos)
                .AsQueryable();

            switch(query.MessageType)
            {
                case "Inbox":
                    messages = messages.Where(u => u.ReceiverID == query.UserID && u.ReceiverDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderID == query.UserID && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.ReceiverID == query.UserID 
                        && u.ReceiverDeleted == false && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(u => u.SendDate);

            var messageList = await PageList<Messages>.CreateAsync(query.PageNumber, query.PageSize, messages);

            return messageList;
        }

        public async Task<IEnumerable<Messages>> GetMessageThread(int userid, int receiverid)
        {
            var messages = await _dbContext.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Receiver).ThenInclude(p => p.Photos)
                .Where(u => u.ReceiverID == userid && u.ReceiverDeleted == false && u.SenderID == receiverid ||
                        u.SenderID == userid && u.SenderDeleted == false && u.ReceiverID == receiverid)
                .OrderByDescending(u => u.SendDate)
                .ToListAsync();

            return messages;
        }

        public async Task<Photos> GetPhoto(int id)
        {
           /* var photo = await Task.Factory.StartNew(() =>
            {
                var t = (from query in _dbContext.Users
                         where query.Photos.Select(x => x.Id == id).FirstOrDefault()
                         select query.Photos.FirstOrDefault());

                return t;
            });*/

            var photo = await _dbContext.Users
                .Where(z => z.Photos.Select(y => y.Id == id).FirstOrDefault())
                 .Select(x => x.Photos.FirstOrDefault()).FirstOrDefaultAsync();
                
            return photo;
            
        }

        public async Task<Users> GetUser(int id)
        {
            var user = await _dbContext.Users
                        .Include(p => p.Photos)
                        .FirstOrDefaultAsync(x => x.UserID == id);

            return user;
        }

        public async Task<PageList<Users>> GetUsers(UserParamsDto query)
        {
            var userList = _dbContext.Users.Include(p => p.Photos).AsQueryable();

            userList = userList.Where(q => q.UserID != query.UserID);

            if(!string.IsNullOrEmpty(query.Gender))
            {
                userList = userList.Where(q => q.Gender == query.Gender);
            }

            if(query.MinAge != 18 || query.MaxAge != 99)
            {
                var minDOB = DateTime.Now.AddYears(-query.MaxAge- 1);
                var maxDOB = DateTime.Now.AddYears(-query.MinAge);

                userList = userList.Where(q => q.DateofBirth>=minDOB &&q.DateofBirth <= maxDOB);
            }

            query.Orderby = string.IsNullOrEmpty(query.Orderby) ? "lastactive" : query.Orderby;

            switch (query.Orderby)
            {
                case "created":
                    userList = userList.OrderByDescending(q => q.CreatedDate);
                    break;
                default:
                    userList = userList.OrderByDescending(q => q.ActiveDate);
                    break;
            }

            // users who like current user
            if (query.LikedUser)
            {
                userList = userList.Include(q => q.LikedUsers)
                    .Where(q => q.LikedUsers.Select(z=>z.LikedUserID).FirstOrDefault() == query.UserID);
            }

            // users who are liked by current user
            if (query.LikedByUser)
            {
                userList = userList.Include(q => q.LikeByUsers)
                    .Where(q => q.LikeByUsers.Select(z => z.LikeByUserID).FirstOrDefault() == query.UserID);
            }

            var pageList = await PageList<Users>.CreateAsync(query.PageNumber, query.PageSize, userList);

            return pageList;
        }

        public async Task<bool> SaveAll()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
