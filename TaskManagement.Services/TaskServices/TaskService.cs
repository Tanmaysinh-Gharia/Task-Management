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
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskManagement.Services.BaseServices;
using TaskManagement.Services.SettingsStore;

namespace TaskManagement.Services.TaskServices
{

    public class TaskService : BaseService, ITaskService
    {
        public TaskService(
        IHttpContextAccessor httpContextAccessor,
        IResponseHandler responseHandler)
        : base(httpContextAccessor, responseHandler, TaskManagementRoutes.TaskApi)
        {
        }

        public async Task<ApiResponse> CreateTaskAsync(CreateTaskModel model)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.Create)
                .PostJsonAsync(model)
                .ReceiveJson<ApiResponse>();
        }

        public async Task<ApiResponse> UpdateTaskAsync(int id, TaskModel model)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.Update.Replace("{id}", id.ToString()))
                .PutJsonAsync(model)
                .ReceiveJson<ApiResponse>();
        }

        public async Task<ApiResponse> DeleteTaskAsync(int id)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.Delete.Replace("{id}", id.ToString()))
                .DeleteAsync()
                .ReceiveJson<ApiResponse>();
        }

        public async Task<ApiResponse> ChangeStatusAsync(int id, int status)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.ChangeStatus.Replace("{id}", id.ToString()))
                .SetQueryParam("status", status)
                .PutAsync()
                .ReceiveJson<ApiResponse>();
        }

        public async Task<List<TaskListItemViewModel>> GetFilteredTasksAsync(TaskFilterModel model)
        {
            var response = await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.FilteredList)
                .PostJsonAsync(model)
                .ReceiveJson<ApiResponse>();

            return _responseHandler.GetResponse<List<TaskListItemViewModel>>(response);
        }

        public async Task<List<TaskDetailViewModel>> GetTaskHistoryAsync(int taskId)
        {
            var response = await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.History.Replace("{id}", taskId.ToString()))
                .GetAsync()
                .ReceiveJson<ApiResponse>();

            return _responseHandler.GetResponse<List<TaskDetailViewModel>>(response);
        }

    }
}
