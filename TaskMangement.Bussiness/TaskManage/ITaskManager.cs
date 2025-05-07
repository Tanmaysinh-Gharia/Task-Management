using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskStatus = TaskManagement.Core.Enums.TaskStatus;
namespace TaskManagement.Bussiness.TaskManage
{
    public interface ITaskManager
    {
        Task AddTaskAsync(CreateTaskModel model, int creatorId);
        Task UpdateTaskAsync(TaskModel model, int userId, bool isAdmin);
        Task DeleteTaskAsync(int taskId, int deletedByUserId, bool isAdmin);
        Task ChangeTaskStatusAsync(int id, TaskStatus status, int userId, bool isAdmin);
        Task<List<TaskListItemViewModel>> GetFilteredTasksAsync(TaskFilterModel model, int userId, bool isAdmin);

        Task<List<TaskDetailViewModel>> GetTaskHistoryAsync(int taskId, int userId, bool isAdmin);

        Task<TaskModel> GetTaskByIdAsync(int taskId, int requesterId, bool isAdmin);

    }

}
