using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Core.ViewModels.Login;
using TaskManagement.Services.LoginServices;

namespace TaskManagement.Web.Controllers
{

    public class AuthenticationController : Controller
    {
        #region Variable Declaration & Assignment in Constructor 
        private readonly ILoginService _loginService;

        public AuthenticationController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the login view.
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Authentication/Login.cshtml");
        }

        /// <summary>
        /// Authenticates user by calling API via LoginService and stores JWT tokens in cookies.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _loginService.LoginAsync(model);

                // Set access token cookie
                Response.Cookies.Append("AccessToken", response.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                // Set refresh token cookie
                Response.Cookies.Append("RefreshToken", response.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                Response.Cookies.Append("IsLoggedIn", "true", new CookieOptions
                {
                    HttpOnly = false, // Can be read in Razor
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });




                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(response.AccessToken);
                var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                TempData["UserRole"] = role; // Pass it to layout

                return role switch
                {
                    "Admin" => RedirectToAction("Home", "Admin"),
                    "User" => RedirectToAction("Home", "User"),
                    _ => RedirectToAction("Login")
                };
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Invalid login credentials");
                return View(model);
            }
        }

        /// <summary>
        /// Clears cookies and logs the user out.
        /// </summary>
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Delete("IsLoggedIn");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
        #endregion
    }

}
