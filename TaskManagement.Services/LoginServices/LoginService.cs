using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.ViewModels.Login;
using Flurl;
using Flurl.Http;

using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Services.SettingsStore;
namespace TaskManagement.Services.LoginServices
{
    public class LoginService(IResponseHandler responseHandler) : ILoginService
    {
        #region Variables Declaration

        private readonly string _apiUrl = $"{Settings.ApiUrl}{AuthenticationManagementRoutes.AuthenticationApi}";
        
        #endregion


        public async Task<AuthenticationResponse>
            LoginAsync(LoginRequest request)
        {
            return responseHandler.GetResponse<AuthenticationResponse>(
                await _apiUrl
                .AppendPathSegment(AuthenticationManagementRoutes.Login)
                .PostJsonAsync(request)
                .ReceiveJson<ApiResponse>());
        }

        public async Task<AuthenticationResponse>
            RefreshTokenAsync(string token)
        {
            return responseHandler.GetResponse<AuthenticationResponse>(
                await _apiUrl
                .AppendPathSegment(AuthenticationManagementRoutes.Refresh)
                .PostJsonAsync(token)
                .ReceiveJson<ApiResponse>());
        }
    }
}
