using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskManagement.Data.Entities;

using TaskStatus = TaskManagement.Core.Enums.TaskStatus;

namespace TaskManagement.Data.Repositories.TaskRepo
{
    public interface ITaskRepository
    {

        Task<TaskEntity?> GetByIdAsync(int id);
        Task AddAsync(TaskEntity task);
        Task UpdateAsync(TaskEntity task);
        Task DeleteAsync(int id);
        Task AddTaskDetailsAsync(List<TaskDetail> details);

        Task<List<TaskDetailViewModel>> GetTaskHistoryAsync(int taskId, int currentUserId, bool isAdmin);

        Task<TaskEntity?> GetWithUserAsync(int taskId);

        Task<List<TaskDetail>> GetTaskDetailsAsync(int taskId);

        Task<List<TaskListItemViewModel>> GetFilteredTasksAsync(
            string? searchTerm,
            int? status,
            int? priority,
            string? sortColumn,
            string? sortOrder,
            int pageNumber,
            int pageSize,
            int userId,
            bool isAdmin);
    }
}
