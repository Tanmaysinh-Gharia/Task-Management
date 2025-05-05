using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ViewModels.Login;
namespace TaskManagement.Bussiness.Authentication
{
    public interface IAuthManager
    {

        Task<AuthenticationResponse> LoginAsync(string email, string password);
        Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken);
    }
}
