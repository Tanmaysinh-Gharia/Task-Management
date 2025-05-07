using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Bussiness.TaskManage;
using TaskManagement.Core.ApiRoutes;
using TaskManagement.Core.Common.ResponseHandler;
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskManagement.Data.Entities;
using TaskManagement.Core.Enums;
using TaskStatus = TaskManagement.Core.Enums.TaskStatus;
namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager _taskManager;
        private readonly ILogger<TaskController> _logger;
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID claim not found."));
        private bool IsAdmin() => User.IsInRole("Admin");

        public TaskController(ITaskManager taskManager, ILogger<TaskController> logger)
        {
            _taskManager = taskManager;
            _logger = logger;
        }



        [HttpPost(TaskManagementRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTaskModel model)
        {
            try
            {
                var userId = GetUserId();
                var creatorId = userId;

                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (!IsAdmin() && model.AssigneeId != creatorId)
                {
                    return BadRequest(ResponseBuilder.Error("You can only assign tasks to yourself."));
                }
                await _taskManager.AddTaskAsync(model, creatorId);
                return Ok(ResponseBuilder.Success("Task created successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create task failed");
                return StatusCode(500, ResponseBuilder.Error("Failed to create task.", ex));
            }
        }



        [HttpPut(TaskManagementRoutes.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] TaskModel model)
        {
            try
            {
                model.Id = id;
                model.UpdatedById = GetUserId();
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                await _taskManager.UpdateTaskAsync(model, model.UpdatedById, IsAdmin());
                return Ok(ResponseBuilder.Success("Task updated successfully."));
            }
            catch (UnauthorizedAccessException uex)
            {
                return Forbid(uex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update task {id} failed");
                return StatusCode(500, ResponseBuilder.Error("Failed to update task.", ex));
            }
        }



        [HttpDelete(TaskManagementRoutes.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetUserId();

                await _taskManager.DeleteTaskAsync(id, userId, IsAdmin());
                return Ok(ResponseBuilder.Success("Task deleted successfully."));
            }
            catch (UnauthorizedAccessException uex)
            {
                return Forbid(uex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete task {id} failed");
                return StatusCode(500, ResponseBuilder.Error("Failed to delete task.", ex));
            }
        }

        [HttpPut(TaskManagementRoutes.ChangeStatus)]
        public async Task<IActionResult> ChangeStatus(int id, [FromQuery] TaskStatus status)
        {
            try
            {
                await _taskManager.ChangeTaskStatusAsync(id, status, GetUserId(), IsAdmin());
                return Ok(ResponseBuilder.Success("Task status updated."));
            }
            catch (UnauthorizedAccessException uex)
            {
                return Forbid(uex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Change status for task {id} failed");
                return StatusCode(500, ResponseBuilder.Error("Failed to change task status.", ex));
            }
        }

        [HttpPost(TaskManagementRoutes.FilteredList)]
        public async Task<IActionResult> GetFilteredList([FromBody] TaskFilterModel model)
        {
            try
            {

                var userId = GetUserId();
                var result = await _taskManager.GetFilteredTasksAsync(model, userId, IsAdmin());
                return Ok(ResponseBuilder.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching filtered tasks failed");
                return StatusCode(500, ResponseBuilder.Error("Error fetching task list.", ex));
            }
        }


        [HttpGet(TaskManagementRoutes.History)]
        public async Task<IActionResult> GetTaskHistory(int id)
        {
            try
            {
                int userId = GetUserId();
                bool isAdmin = IsAdmin();

                var result = await _taskManager.GetTaskHistoryAsync(id, userId, isAdmin);
                return Ok(ResponseBuilder.Success(result));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching task history");
                return StatusCode(500, ResponseBuilder.Error("Error fetching task history", ex));
            }
        }

        [HttpGet(TaskManagementRoutes.GetTask)]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var userId = GetUserId();
                TaskModel task = await _taskManager.GetTaskByIdAsync(id, userId, IsAdmin());
                return Ok(ResponseBuilder.Success(task));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching task");
                return StatusCode(500, ResponseBuilder.Error("Error fetching task", ex));
            }
        }
    }
}
