﻿using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.TaskManagement;

namespace TaskManagement.Services.TaskServices
{
    public interface ITaskService
    {
        Task<ApiResponse> CreateTaskAsync(CreateTaskModel model);
        Task<ApiResponse> UpdateTaskAsync(int id, TaskModel model);
        Task<ApiResponse> DeleteTaskAsync(int id);
        Task<ApiResponse> ChangeStatusAsync(int id, int status);
        Task<List<TaskListItemViewModel>> GetFilteredTasksAsync(TaskFilterModel model);
        Task<List<TaskDetailViewModel>> GetTaskHistoryAsync(int taskId);
        Task<TaskModel> GetTaskByIdAsync(int id);

    }
}
