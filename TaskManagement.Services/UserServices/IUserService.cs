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
        Task<UserViewModel> GetUserByIdAsync(int id);
    }
}
