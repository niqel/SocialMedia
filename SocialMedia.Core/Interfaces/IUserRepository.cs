using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task CreateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<bool> UpdateUser(User user);
    }
}
