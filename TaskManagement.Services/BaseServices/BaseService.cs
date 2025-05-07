using Flurl.Http;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Services.SettingsStore;

namespace TaskManagement.Services.BaseServices
{
    public abstract class BaseService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IResponseHandler _responseHandler;
        protected readonly string _baseApiUrl;

        protected BaseService(
            IHttpContextAccessor httpContextAccessor,
            IResponseHandler responseHandler,
            string apiRoute)
        {
            _httpContextAccessor = httpContextAccessor;
            _responseHandler = responseHandler;
            _baseApiUrl = $"{Settings.ApiUrl}{apiRoute}";
        }

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
