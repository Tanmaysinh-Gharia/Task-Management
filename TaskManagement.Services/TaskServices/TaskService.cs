using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskManagement.Services.BaseServices;

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

        /// <summary>
        /// Sends a request to the API to create a new task using the provided model.
        /// </summary>
        public async Task<ApiResponse> CreateTaskAsync(CreateTaskModel model)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.Create)
                .PostJsonAsync(model)
                .ReceiveJson<ApiResponse>();
        }


        /// <summary>
        /// Sends a request to the API to update an existing task identified by ID.
        /// </summary>
        public async Task<ApiResponse> UpdateTaskAsync(int id, TaskModel model)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.Update.Replace("{id}", id.ToString()))
                .PutJsonAsync(model)
                .ReceiveJson<ApiResponse>();
        }


        /// <summary>
        /// Sends a request to the API to delete a task by ID.
        /// </summary>
        public async Task<ApiResponse> DeleteTaskAsync(int id)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.Delete.Replace("{id}", id.ToString()))
                .DeleteAsync()
                .ReceiveJson<ApiResponse>();
        }

        /// <summary>
        /// Sends a request to the API to change the status of a task by ID.
        /// </summary>
        public async Task<ApiResponse> ChangeStatusAsync(int id, int status)
        {
            return await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.ChangeStatus.Replace("{id}", id.ToString()))
                .SetQueryParam("status", status)
                .PutAsync()
                .ReceiveJson<ApiResponse>();
        }


        /// <summary>
        /// Sends a request to the API to get a filtered, sorted, and paginated list of tasks.
        /// </summary>
        public async Task<List<TaskListItemViewModel>> GetFilteredTasksAsync(TaskFilterModel model)
        {
            var response = await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.FilteredList)
                .PostJsonAsync(model)
                .ReceiveJson<ApiResponse>();

            return _responseHandler.GetResponse<List<TaskListItemViewModel>>(response);
        }


        /// <summary>
        /// Sends a request to the API to retrieve the full history log of a specific task.
        /// </summary>
        public async Task<List<TaskDetailViewModel>> GetTaskHistoryAsync(int taskId)
        {
            var response = await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.History.Replace("{id}", taskId.ToString()))
                .GetAsync()
                .ReceiveJson<ApiResponse>();

            return _responseHandler.GetResponse<List<TaskDetailViewModel>>(response);
        }


        /// <summary>
        /// Sends a request to the API to retrieve detailed information about a task by ID.
        /// </summary>
        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            var response = await GetAuthenticatedRequest()
                .AppendPathSegment(TaskManagementRoutes.GetTask.Replace("{id}", id.ToString()))
                .GetAsync()
                .ReceiveJson<ApiResponse>();

            return _responseHandler.GetResponse<TaskModel>(response);
        }

    }
}
