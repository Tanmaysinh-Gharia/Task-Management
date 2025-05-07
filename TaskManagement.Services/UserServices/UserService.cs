using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.UserManagement;
using TaskManagement.Services.BaseServices;
using TaskManagement.Services.SettingsStore;

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

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var response = await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.GetUsers)
                .GetAsync()
                .ReceiveJson<ApiResponse>();

            return _responseHandler.GetResponse<List<UserViewModel>>(response);
        }

        public async Task<ApiResponse> CreateUserAsync(CreateUserModel model)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.AddUser)
                .PostJsonAsync(model)
                .ReceiveJson<ApiResponse>();
        }

        public async Task<ApiResponse> UpdateUserAsync(UserViewModel model)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.UpdateUser)
                .PutJsonAsync(model)
                .ReceiveJson<ApiResponse>();
        }

        public async Task<ApiResponse> DeleteUserAsync(int id)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(UserManagementRoutes.DeleteUser.Replace("{id}", id.ToString()))
                .DeleteAsync()
                .ReceiveJson<ApiResponse>();
        }
    }
}
