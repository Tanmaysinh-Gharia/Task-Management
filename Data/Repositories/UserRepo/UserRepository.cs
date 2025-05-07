using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Context;
using TaskManagement.Data.Entities;

namespace TaskManagement.Data.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user by their email address. Returns null if not found.
        /// </summary>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves a user by their unique ID. Returns null if not found.
        /// </summary>
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }


        /// <summary>
        /// Retrieves a list of all users from the database.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Adds a new user entity to the database and saves changes.
        /// </summary>
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing user entity and persists changes to the database.
        /// </summary>
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a user entity from the database and saves changes.
        /// </summary>
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Checks whether a user with the specified ID exists in the database.
        /// </summary>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }
        
    }
}
