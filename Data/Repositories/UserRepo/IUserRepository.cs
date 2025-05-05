using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Entities;

namespace TaskManagement.Data.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);

        //Task<IList<UserName>> GetUserNames();
        // For CRUD by ADMIN

        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistsAsync(int id);
    }
}
