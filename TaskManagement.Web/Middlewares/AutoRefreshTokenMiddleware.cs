using Flurl;
using System.IdentityModel.Tokens.Jwt;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.Login;
using TaskManagement.Services.LoginServices;
using TaskManagement.Services.SettingsStore;
using static System.Formats.Asn1.AsnWriter;

namespace TaskManagement.Web.Middlewares
{
    /// <summary>
    /// Middleware to automatically refresh expired JWT access tokens using refresh token.
    /// Tokens are stored in secure cookies and refreshed via the login service layer.
    /// </summary>
    public class AutoRefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private ILoginService _loginService;

        private readonly IServiceScopeFactory _scopeFactory;
        /// <summary>
        /// Constructor injecting next middleware and login service
        /// </summary>
        public AutoRefreshTokenMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Middleware invocation logic
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            _loginService = scope.ServiceProvider.GetRequiredService<ILoginService>();

            var accessToken = context.Request.Cookies["AccessToken"];
            var refreshToken = context.Request.Cookies["RefreshToken"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);

                if (token.ValidTo > DateTime.Now)
                {
                    try
                    {
                        var newTokens = await _loginService.RefreshTokenAsync(refreshToken!);

                        context.Response.Cookies.Append("AccessToken", newTokens.AccessToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });

                        context.Response.Cookies.Append("RefreshToken", newTokens.RefreshToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });
                    }
                    catch
                    {
                        context.Response.Cookies.Delete("AccessToken");
                        context.Response.Cookies.Delete("RefreshToken");
                        context.Response.Redirect("/Authentication/Login"); // Fixed path
                        return;
                    }
                }
            }

            await _next(context);
        }

    }
}
