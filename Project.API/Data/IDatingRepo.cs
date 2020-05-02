using Project.API.Dtos;
using Project.API.Helpers;
using Project.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Data
{
    public interface IDatingRepo
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<PageList<Users>> GetUsers(UserParamsDto query);

        Task<Users> GetUser(int id);

        Task<Photos> GetPhoto(int id);

        Task<Likes> GetLikes(int userid, int likeuserid);

        Task<Messages> GetMessage(int messageid);

        Task<PageList<Messages>> GetMessagesForUser(MessageParamsDto query);

        Task<IEnumerable<Messages>> GetMessageThread(int userid, int receiverid);
    }
}
