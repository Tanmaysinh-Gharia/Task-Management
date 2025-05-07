using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
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
        
        [HttpPost(AuthenticationManagementRoutes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authManager.LoginAsync(request.Email, request.Password);
                return Ok(ResponseBuilder.Success(response));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
        
        [HttpPost(AuthenticationManagementRoutes.Refresh)]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            try
            {
                var response = await _authManager.RefreshTokenAsync(token);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet("getdata")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data["Users"] = "Log";
                return Ok(data);
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}
