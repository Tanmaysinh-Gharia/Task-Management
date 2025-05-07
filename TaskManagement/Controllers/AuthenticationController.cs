using Microsoft.AspNetCore.Mvc;
using TaskManagement.Bussiness.Authentication;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.Login;
using LoginRequest = TaskManagement.Core.ViewModels.Login.LoginRequest;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        public AuthenticationController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        /// <summary>
        /// Authenticates the user using provided email and password,
        /// and returns a JWT access and refresh token if valid.
        /// </summary>
        /// <param name="request">LoginRequest containing Email and Password</param>
        /// <returns>200 OK with tokens if successful; 401 Unauthorized if invalid credentials</returns>

        [HttpPost(AuthenticationManagementRoutes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                AuthenticationResponse? response = await _authManager.LoginAsync(request.Email, request.Password);
                return Ok(ResponseBuilder.Success(response));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }


        /// <summary>
        /// Refreshes the expired access token using a valid refresh token.
        /// </summary>
        /// <param name="token">Refresh token string</param>
        /// <returns>200 OK with new access and refresh tokens; 401 Unauthorized if refresh token is invalid</returns>

        [HttpPost(AuthenticationManagementRoutes.Refresh)]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            try
            {
                AuthenticationResponse? response = await _authManager.RefreshTokenAsync(token);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

    }
}
