using TaskManagement.Core.ViewModels.UserManagement;

namespace TaskManagement.Bussiness.UserManage
{
    public interface IUserManager
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();

        Task<UserModel?> GetUserByIdAsync(int id);
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task AddUserAsync(CreateUserModel model);
        Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int id);
        Task UpdateUserAsync(UserViewModel userModel);
        Task<UserViewModel> ShowUserByIdAsync(int id);
    }
}
