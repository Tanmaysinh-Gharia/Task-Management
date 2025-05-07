using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.UserManagement;

namespace TaskManagement.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetAllUsersAsync();
        Task<ApiResponse> CreateUserAsync(CreateUserModel model);
        Task<ApiResponse> UpdateUserAsync(UserViewModel model);
        Task<ApiResponse> DeleteUserAsync(int id);
    }
}
