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
    [Authorize(Roles = "Admin")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager _taskManager;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskManager taskManager, ILogger<TaskController> logger)
        {
            _taskManager = taskManager;
            _logger = logger;
        }
        [Authorize]
        [HttpPost(TaskManagementRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTaskModel model)
        {
            try
            {
                var creatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID claim not found."));
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if ( role == Role.User.ToString() && model.AssigneeId != creatorId)
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
        public async Task<IActionResult> Update(
            int id, 
            [FromBody] TaskModel model)
        {
            try
            {
                model.Id = id;
                model.UpdatedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                       throw new InvalidOperationException("User identity not found."));
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                await _taskManager.UpdateTaskAsync(model);
                return Ok(ResponseBuilder.Success("Task updated successfully."));
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
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID claim not found."));
                await _taskManager.DeleteTaskAsync(id,userId);
                return Ok(ResponseBuilder.Success("Task deleted successfully."));
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
                await _taskManager.ChangeTaskStatusAsync(id, status);
                return Ok(ResponseBuilder.Success("Task status updated."));
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
                var result = await _taskManager.GetFilteredTasksAsync(model);
                return Ok(ResponseBuilder.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching filtered tasks failed");
                return StatusCode(500, ResponseBuilder.Error("Error fetching task list.", ex));
            }
        }
    }
}
