using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskManagement.Data.Context;
using TaskManagement.Data.Entities;
namespace TaskManagement.Data.Repositories.TaskRepo
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TaskEntity?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task AddAsync(TaskEntity task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                task.IsDeleted = true;
                await UpdateAsync(task);
            }
        }


        public async Task AddTaskDetailsAsync(List<TaskDetail> details)
        {
            await _context.TaskDetails.AddRangeAsync(details);
            await _context.SaveChangesAsync();
        }


        public async Task<List<TaskListItemViewModel>> GetFilteredTasksAsync(
            string? searchTerm,
            int? status,
            int? priority,
            string? sortColumn,
            string? sortOrder,
            int pageNumber,
            int pageSize,
            int userId,
            bool isAdmin)
        {

            return await _context.TaskListItemViewModel.FromSqlInterpolated($@"
                                    EXEC sp_GetFilteredTasks 
                                        {searchTerm}, 
                                        {status}, 
                                        {priority}, 
                                        {sortColumn ?? "CreatedAt"}, 
                                        {sortOrder ?? "ASC"}, 
                                        {pageNumber}, 
                                        {pageSize},
                                        {userId},
                                        {isAdmin}")
                                .ToListAsync();
        }


        public async Task<List<TaskDetailViewModel>> GetTaskHistoryAsync(int taskId, int currentUserId, bool isAdmin)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || task.IsDeleted) return new();

            bool isPersonalTask = task.CreatorId == task.AssigneeId;

            if (!isAdmin)
            {
                if (isPersonalTask && task.AssigneeId != currentUserId)
                    throw new UnauthorizedAccessException("Access denied to personal task history.");
                if (!isPersonalTask && task.AssigneeId != currentUserId)
                    throw new UnauthorizedAccessException("Access denied to this task.");
            }

            return await (from td in _context.TaskDetails
                          join u in _context.Users on td.UpdatedById equals u.Id
                          where td.TaskId == taskId
                          orderby td.ChangeTime
                          select new TaskDetailViewModel
                          {
                              FieldName = td.FieldName,
                              OldValue = td.OldValue,
                              NewValue = td.NewValue,
                              UpdatedBy = u.UserName,
                              ChangeTime = td.ChangeTime
                          }).ToListAsync();
        }


        public async Task<TaskEntity?> GetWithUserAsync(int taskId)
        {
            return await _context.Tasks
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);
        }

        public async Task<List<TaskDetail>> GetTaskDetailsAsync(int taskId)
        {
            return await _context.TaskDetails
                .Include(td => td.UpdatedBy)
                .Where(td => td.TaskId == taskId)
                .OrderBy(td => td.ChangeTime)
                .ToListAsync();
        }

    }
}
