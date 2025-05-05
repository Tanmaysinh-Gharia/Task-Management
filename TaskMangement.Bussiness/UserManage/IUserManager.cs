using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ViewModels.UserManagement;

namespace TaskManagement.Bussiness.UserManage
{
    public interface IUserManager
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();

        Task<UserModel?> GetUserByIdAsync(int id);
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task AddUserAsync(UserModel user);
        Task AddUserAsync(CreateUserModel model);

        Task UpdateUserAsync(UserModel user);
        Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int id);
        Task UpdateUserAsync(UserViewModel userModel);
        Task<UserViewModel> ShowUserByIdAsync(int id);
    }
}
