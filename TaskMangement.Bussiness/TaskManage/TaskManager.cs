using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Common.Configuration;
using TaskManagement.Core.InjectionInterfaces;
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskManagement.Data.Entities;
using TaskManagement.Data.Repositories.TaskRepo;
using TaskManagement.Data.Repositories.UserRepo;
using TaskStatus = TaskManagement.Core.Enums.TaskStatus;
namespace TaskManagement.Bussiness.TaskManage
{
    public class TaskManager : ITaskManager
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationSettings _paginationSettings;

        public TaskManager(IMapper mapper, ITaskRepository repo, IUserRepository userRepo, IOptions<PaginationSettings> paginationSettings,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taskRepo = repo;
            _userRepo = userRepo;
            _paginationSettings = paginationSettings.Value;
        }


        public async Task<List<TaskListItemViewModel>> GetFilteredTasksAsync(TaskFilterModel model, int userId, bool isAdmin)
        {
            // Fallback to config values if not provided
            var pageNumber = model.PageNumber > 0 ? model.PageNumber : _paginationSettings.DefaultPageNumber;
            var pageSize = model.PageSize > 0 ? model.PageSize : _paginationSettings.DefaultPageSize;

            return await _taskRepo.GetFilteredTasksAsync(
                model.SearchTerm,
                model.Status.HasValue ? (int)model.Status.Value : (int?)null,
                model.Priority.HasValue ? (int)model.Priority.Value : (int?)null,
                model.SortColumn ?? "CreatedAt",
                model.SortOrder ?? "ASC",
                pageNumber,
                pageSize,
                userId,
                isAdmin
            );
        }

        public async Task AddTaskAsync(CreateTaskModel model, int creatorId)
        {
            var entity = _mapper.Map<TaskEntity>(model);
            entity.CreatorId = creatorId;
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = entity.CreatedAt;

            await _taskRepo.AddAsync(entity);
        }

        public async Task UpdateTaskAsync(TaskModel model, int userId, bool isAdmin)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var task = await _taskRepo.GetByIdAsync(model.Id);
                if (task == null)
                    throw new Exception("Task not found");

                if (isAdmin && task.CreatorId == task.AssigneeId)
                    throw new UnauthorizedAccessException("Admins cannot update personal tasks.");

                if (isAdmin && task.CreatorId == task.AssigneeId)
                    throw new UnauthorizedAccessException("Admins cannot update personal tasks.");


                var now = DateTime.Now;
                var changes = new List<TaskDetail>();

                if (!string.IsNullOrWhiteSpace(model.Title) && model.Title != task.Title)
                {
                    changes.Add(CreateDetail(task.Id, model.UpdatedById, "Title", task.Title, model.Title, now));
                    task.Title = model.Title;
                }

                if (!string.IsNullOrWhiteSpace(model.Description) && model.Description != task.Description)
                {
                    changes.Add(CreateDetail(task.Id, model.UpdatedById, "Description", task.Description, model.Description, now));
                    task.Description = model.Description;
                }

                if (model.Status.HasValue && model.Status.Value != task.Status)
                {
                    changes.Add(CreateDetail(task.Id, model.UpdatedById, "Status", task.Status.ToString(), model.Status.ToString(), now));
                    task.Status = model.Status.Value;
                }

                if (model.Priority.HasValue && model.Priority.Value != task.Priority)
                {
                    changes.Add(CreateDetail(task.Id, model.UpdatedById, "Priority", task.Priority.ToString(), model.Priority.ToString(), now));
                    task.Priority = model.Priority.Value;
                }

                if (model.DueDate.HasValue && model.DueDate.Value != task.DueDate)
                {
                    changes.Add(CreateDetail(task.Id, model.UpdatedById, "DueDate", task.DueDate.ToString("s"), model.DueDate.Value.ToString("s"), now));
                    task.DueDate = model.DueDate.Value;
                }

                if (model.AssigneeId.HasValue && model.AssigneeId.Value != task.AssigneeId)
                {
                    changes.Add(CreateDetail(task.Id, model.UpdatedById, "AssigneeId", task.AssigneeId.ToString(), model.AssigneeId.Value.ToString(), now));
                    task.AssigneeId = model.AssigneeId.Value;
                }

                task.UpdatedAt = now;

                await _taskRepo.UpdateAsync(task);
                if (changes.Count > 0)
                    await _taskRepo.AddTaskDetailsAsync(changes);

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteTaskAsync(int taskId, int deletedByUserId, bool isAdmin)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                int userId = deletedByUserId;
                var task = await _taskRepo.GetByIdAsync(taskId);
                if (task == null || task.IsDeleted)
                    throw new Exception("Task not found or already deleted");


                if (isAdmin && task.CreatorId == task.AssigneeId)
                    throw new UnauthorizedAccessException("Admins cannot delete personal tasks.");

                if (!isAdmin && (task.CreatorId != userId || task.AssigneeId != userId))
                    throw new UnauthorizedAccessException("Users can only delete their own personal tasks.");

                task.IsDeleted = true;
                task.UpdatedAt = DateTime.Now;

                await _taskRepo.UpdateAsync(task);

                var history = new TaskDetail
                {
                    TaskId = task.Id,
                    UpdatedById = deletedByUserId,
                    FieldName = "IsDeleted",
                    OldValue = "false",
                    NewValue = "true",
                    ChangeTime = DateTime.Now
                };

                await _taskRepo.AddTaskDetailsAsync(new List<TaskDetail> { history });

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }


        public async Task ChangeTaskStatusAsync(int id, TaskStatus status, int userId, bool isAdmin)
        {
            //var task = await _taskRepo.GetByIdAsync(id);
            //if (task == null || task.IsDeleted) throw new Exception("Task not found");


            //if (!isAdmin && task.AssigneeId != userId)
            //    throw new UnauthorizedAccessException("You can only change status of your assigned tasks.");

            //task.Status = status;
            //task.UpdatedAt = DateTime.Now;
            //await _taskRepo.UpdateAsync(task);





            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var task = await _taskRepo.GetByIdAsync(id);
                if (task == null || task.IsDeleted) throw new Exception("Task not found");


                if (!isAdmin && task.AssigneeId != userId)
                    throw new UnauthorizedAccessException("You can only change status of your assigned tasks.");

                var now = DateTime.Now;
                var changes = new List<TaskDetail>();
                
                if(status == null)
                {
                    throw new Exception("Status cannot be null");
                }

                if (status != null && status != task.Status)
                {
                    changes.Add(CreateDetail(task.Id, userId, "Status", task.Status.ToString(), status.ToString(), now));
                    task.Status = status;
                }
                else
                {
                    throw new Exception("Status is the same as current status");
                }

                task.UpdatedAt = now;

                await _taskRepo.UpdateAsync(task);

                if (changes.Count > 0)
                    await _taskRepo.AddTaskDetailsAsync(changes);

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }


        public async Task<List<TaskDetailViewModel>> GetTaskHistoryAsync(int taskId, int requesterId, bool isAdmin)
        {
            var task = await _taskRepo.GetWithUserAsync(taskId);
            if (task == null || task.IsDeleted)
                throw new Exception("Task not found");

            // Authorization checks
            if (!isAdmin)
            {
                bool isPersonal = task.CreatorId == task.AssigneeId && task.CreatorId == requesterId;
                bool isAssignedToUser = task.AssigneeId == requesterId;

                if (!isPersonal && !isAssignedToUser)
                    throw new UnauthorizedAccessException("User not allowed to view this task history.");
            }

            var history = new List<TaskDetailViewModel>();

            // 1️ Add creation snapshot
            history.Add(new TaskDetailViewModel
            {
                FieldName = "Created",
                OldValue = null,
                NewValue = $"Title: {task.Title}, Description: {task.Description}, Status: {task.Status}, Priority: {task.Priority}, DueDate: {task.DueDate}, AssignedTo: {task.Assignee?.UserName}",
                UpdatedBy = task.Creator?.UserName ?? "Unknown",
                ChangeTime = task.CreatedAt
            });

            // 2️ Add updates & deletion logs
            var details = await _taskRepo.GetTaskDetailsAsync(taskId);

            history.AddRange(details.Select(d => new TaskDetailViewModel
            {
                FieldName = d.FieldName,
                OldValue = d.OldValue,
                NewValue = d.NewValue,
                UpdatedBy = d.UpdatedBy?.UserName ?? "System",
                ChangeTime = d.ChangeTime
            }));

            return history.OrderBy(h => h.ChangeTime).ToList();
        }



        #region Private Methods
        private TaskDetail CreateDetail(int taskId, int updaterId, string field, string? oldVal, string? newVal, DateTime time)
        {
            return new TaskDetail
            {
                TaskId = taskId,
                UpdatedById = updaterId,
                FieldName = field,
                OldValue = oldVal,
                NewValue = newVal,
                ChangeTime = time
            };
        }
        #endregion
    }

}
