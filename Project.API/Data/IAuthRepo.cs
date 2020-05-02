using Project.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Data
{
    public interface IAuthRepo
    {
        Task<Users> Login(string username, string password);

        Task<Users> Register(Users user, string password);

        Task<bool> UserExists(string username);

        void SaveAllChanges();
    }
}
