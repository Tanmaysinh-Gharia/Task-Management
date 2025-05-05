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
        Task UpdateTaskAsync(TaskModel model);
        Task DeleteTaskAsync(int taskId, int deletedByUserId);
        Task ChangeTaskStatusAsync(int id, TaskStatus status);
        Task<List<TaskListItemViewModel>> GetFilteredTasksAsync(TaskFilterModel model);

    }

}
