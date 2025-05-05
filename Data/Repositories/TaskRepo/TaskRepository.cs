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
            int pageSize)
        {

            return await _context.TaskListItemViewModel.FromSqlInterpolated($@"
                                    EXEC sp_GetFilteredTasks 
                                        {searchTerm}, 
                                        {status}, 
                                        {priority}, 
                                        {sortColumn ?? "CreatedAt"}, 
                                        {sortOrder ?? "ASC"}, 
                                        {pageNumber}, 
                                        {pageSize}")
                                .ToListAsync();
        }
    }
}
