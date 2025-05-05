using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TaskManagement.Core.ViewModels.Login;

namespace TaskManagement.API.Middlewares.Authentication
{
    public class AutoRefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _key;
        public AutoRefreshTokenMiddleware(IConfiguration configuration, RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _next = next;
            _httpClientFactory = httpClientFactory;
            _key = _configuration.GetSection("Jwt:Key").Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authHeader))
            {
                var token = authHeader.Substring("Bearer ".Length);

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    // Validate token manually
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromSeconds(5),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)) // match jwt:Key
                    }, out _);
                }
                catch (SecurityTokenExpiredException)
                {
                    var refreshToken = context.Request.Headers["X-Refresh-Token"].FirstOrDefault();
                    if (string.IsNullOrEmpty(refreshToken))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Refresh token missing or invalid.");
                        return;
                    }

                    var client = _httpClientFactory.CreateClient();
                    var refreshResponse = await client.PostAsJsonAsync("https://localhost:7188/api/auth/refresh", refreshToken);

                    if (!refreshResponse.IsSuccessStatusCode)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token refresh failed.");
                        return;
                    }

                    var tokenResponse = await refreshResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();
                    if (tokenResponse == null)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    // Replace token in header
                    context.Request.Headers["Authorization"] = $"Bearer {tokenResponse.AccessToken}";
                    context.Request.Headers["X-Refresh-Token"] = tokenResponse.RefreshToken;
                }
            }

            await _next(context);
        }
    }
}