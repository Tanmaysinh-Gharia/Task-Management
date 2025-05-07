using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ViewModels.Login;

namespace TaskManagement.Services.LoginServices
{
    public interface ILoginService
    {
        Task<AuthenticationResponse> LoginAsync(LoginRequest model);
        Task<AuthenticationResponse> RefreshTokenAsync(string token);
    }
}
