using Flurl.Http;
using Microsoft.AspNetCore.Http;
using TaskManagement.Services.SettingsStore;

namespace TaskManagement.Services.BaseServices
{
    public abstract class BaseService
    {
        #region Variables Declaration
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IResponseHandler _responseHandler;
        protected readonly string _baseApiUrl;
        #endregion


        /// <summary>
        /// Abstract base service that provides shared utilities such as authenticated request creation,
        /// base API URL construction, and response handling support.
        /// </summary>
        protected BaseService(
            IHttpContextAccessor httpContextAccessor,
            IResponseHandler responseHandler,
            string apiRoute)
        {
            _httpContextAccessor = httpContextAccessor;
            _responseHandler = responseHandler;
            _baseApiUrl = $"{Settings.ApiUrl}{apiRoute}";
        }


        /// <summary>
        /// Builds a Flurl HTTP request with access and refresh tokens from the current HTTP context cookies.
        /// Adds them as headers to authorize API calls.
        /// </summary>
        protected IFlurlRequest GetAuthenticatedRequest()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];

            return _baseApiUrl
                .WithHeader("Authorization", $"Bearer {accessToken}")
                .WithHeader("X-Refresh-Token", refreshToken);
        }
    }
}
