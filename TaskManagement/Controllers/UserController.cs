using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Bussiness.UserManage;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.UserManagement;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// All user apis are only accessible by admin 
    ///</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserManager userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all users. Only accessible by Admins.
        /// </summary>
        [HttpGet(UserManagementRoutes.GetUsers)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                IEnumerable<UserViewModel> users = await _userManager.GetAllUsersAsync();
                return Ok(ResponseBuilder.Success(users));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users.");
                return StatusCode(500, ResponseBuilder.Error("An error occurred while retrieving users.", ex));
            }
        }

        /// <summary>
        /// Adds a new user to the system. Only accessible by Admins.
        /// </summary>
        [HttpPost(UserManagementRoutes.AddUser)]
        public async Task<IActionResult> AddUser([FromBody] CreateUserModel model)
        {
            try
            {
                await _userManager.AddUserAsync(model);
                return Ok(ResponseBuilder.Success("User added successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user.");
                return StatusCode(500, ResponseBuilder.Error("An error occurred while adding the user.", ex));
            }
        }

        /// <summary>
        /// Updates an existing user's details. Only accessible by Admins.
        /// </summary>
        [HttpPut(UserManagementRoutes.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel userModel)
        {
            try
            {
                await _userManager.UpdateUserAsync(userModel);
                return Ok(ResponseBuilder.Success("User updated successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user.");
                return StatusCode(500, ResponseBuilder.Error("An error occurred while updating the user.", ex));
            }
        }


        /// <summary>
        /// Deletes a user by ID after checking existence. Only accessible by Admins.
        /// </summary>
        [HttpDelete(UserManagementRoutes.DeleteUser)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool exists = await _userManager.UserExistsAsync(id);
                if (!exists)
                    return NotFound(ResponseBuilder.NotFound("User not found."));

                await _userManager.DeleteUserAsync(id);
                return Ok(ResponseBuilder.Success("User deleted successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID: {id}.");
                return StatusCode(500, ResponseBuilder.Error("An error occurred while deleting the user.", ex));
            }
        }
    
    }
}
