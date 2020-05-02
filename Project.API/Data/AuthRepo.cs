using Microsoft.EntityFrameworkCore;
using Project.API.Helpers;
using Project.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Data
{
    public class AuthRepo : IAuthRepo
    {
        private readonly MyDbContext _dbContext;

        public AuthRepo(MyDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<Users> Login(string username, string password)
        {
            var user = await _dbContext.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == username);

            if(user == null)
            {
                return null;
            }

            AuthClass objAuth = new AuthClass();
            if (!objAuth.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<Users> Register(Users user, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            AuthClass objAuth = new AuthClass();
            objAuth.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if(user != null)
            {
                return true;
            }

            return false;

        }

        public async void SaveAllChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
