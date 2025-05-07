using TaskManagement.Core.ViewModels.Login;

namespace TaskManagement.Services.LoginServices
{
    public interface ILoginService
    {
        Task<AuthenticationResponse> LoginAsync(LoginRequest model);
        Task<AuthenticationResponse> RefreshTokenAsync(string token);
    }
}
