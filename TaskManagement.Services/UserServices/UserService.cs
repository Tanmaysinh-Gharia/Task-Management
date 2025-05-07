using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.UserManagement;
using TaskManagement.Services.BaseServices;

namespace TaskManagement.Services.UserServices
{
    public class UserService : BaseService, IUserService
    {
        public UserService(
            IHttpContextAccessor httpContextAccessor,
            IResponseHandler responseHandler)
            : base(httpContextAccessor, responseHandler, UserManagementRoutes.UserApi)
        {
        }

        /// <summary>
        /// Sends a request to retrieve all users. Returns a list of UserViewModel objects.
        /// </summary>
        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var response = await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.GetUsers)
                .GetAsync()
                .ReceiveJson<ApiResponse>();

            return _responseHandler.GetResponse<List<UserViewModel>>(response);
        }

        /// <summary>
        /// Sends a request to create a new user using the provided CreateUserModel.
        /// </summary>
        public async Task<ApiResponse> CreateUserAsync(CreateUserModel model)
        {
            string api = UserManagementRoutes.AddUser;
            return await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.AddUser)
                .PostJsonAsync(model)
                .ReceiveJson<ApiResponse>();
        }


        /// <summary>
        /// Sends a request to update an existing user using the provided UserViewModel.
        /// </summary>
        public async Task<ApiResponse> UpdateUserAsync(UserViewModel model)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.UpdateUser)
                .PutJsonAsync(model)
                .ReceiveJson<ApiResponse>();
        }

        /// <summary>
        /// Sends a request to delete a user identified by the given ID.
        /// </summary>
        public async Task<ApiResponse> DeleteUserAsync(int id)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.DeleteUser.Replace("{id}", id.ToString()))
                .DeleteAsync()
                .ReceiveJson<ApiResponse>();
        }

        /// <summary>
        /// Sends a request to retrieve a user by their ID. Returns a UserViewModel.
        /// </summary>
        public async Task<UserViewModel> GetUserByIdAsync(int id)
        {
            var response = await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.GetUserById.Replace("{id}", id.ToString()))
                .GetAsync()
                .ReceiveJson<ApiResponse>();

            return _responseHandler.GetResponse<UserViewModel>(response);
        }
    }
}
