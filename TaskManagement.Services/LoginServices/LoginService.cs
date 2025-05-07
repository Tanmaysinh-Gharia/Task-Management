using Flurl;
using Flurl.Http;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.Login;
using TaskManagement.Services.SettingsStore;
namespace TaskManagement.Services.LoginServices
{
    public class LoginService(IResponseHandler responseHandler) : ILoginService
    {
        #region Variables Declaration

        private readonly string _apiUrl = $"{Settings.ApiUrl}{AuthenticationManagementRoutes.AuthenticationApi}";

        #endregion

        /// <summary>
        /// Sends login request to the API and returns authentication tokens on success.
        /// Uses Flurl to post JSON data and extract the response using the response handler.
        /// </summary>
        public async Task<AuthenticationResponse>
            LoginAsync(LoginRequest request)
        {
            return responseHandler.GetResponse<AuthenticationResponse>(
                await _apiUrl
                .AppendPathSegment(AuthenticationManagementRoutes.Login)
                .PostJsonAsync(request)
                .ReceiveJson<ApiResponse>());
        }

        /// <summary>
        /// Sends refresh token to the API and retrieves new access and refresh tokens.
        /// </summary>
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
