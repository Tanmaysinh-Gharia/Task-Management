using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskManagement.Core.ViewModels.UserManagement;
using TaskManagement.Services.TaskServices;
using TaskManagement.Services.UserServices;

namespace TaskManagement.Web.Controllers
{
    public class AdminController : Controller
    {
        #region Services

        private readonly ITaskService _taskService;
        private readonly IUserService _userService; 

        public AdminController(ITaskService taskService,IUserService userService)
        {
            _userService = userService;
            _taskService = taskService;
        }

        #endregion

        #region Dashboard Navigation

        [HttpGet]
        public IActionResult Home()
        {
            return View("~/Views/Admin/Home.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Task()
        {
            List<TaskListItemViewModel> tasks = await _taskService.GetFilteredTasksAsync(new TaskFilterModel());
            return View("~/Views/Admin/Task/Index.cshtml",tasks);
        }

        [HttpGet]
        public async Task<IActionResult> User()
        {
            List<UserViewModel> users = await _userService.GetAllUsersAsync();
            return View("~/Views/Admin/User/Index.cshtml",users);
        }

        #endregion

        #region Task Views

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("~/Views/Admin/Task/_AddEditTask.cshtml", new CreateTaskModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                return PartialView("~/Views/Admin/Task/_AddEditTask.cshtml",task);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("You are not authorized to view this task.");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error retrieving task");
                return StatusCode(500, "An error occurred while retrieving the task.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ViewTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return PartialView("~/Views/Admin/Task/_ViewTask.cshtml",task);
        }

        [HttpGet]
        public async Task<IActionResult> History(int id)
        {
            var history = await _taskService.GetTaskHistoryAsync(id);
            return PartialView("~/Views/Admin/Task/_TaskHistory.cshtml", history);
        }


        #endregion

        #region Task API Calls

        [HttpPost]
        public async Task<IActionResult> Create( CreateTaskModel model)
        {
            if (model == null)
                return BadRequest("Model is null");
            await _taskService.CreateTaskAsync(model);

            return Ok(new { success = true, message = "Task created successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(TaskModel model)
        {
            var response = await _taskService.UpdateTaskAsync(model.Id, model);
            return  Ok(new { success = true, message = "Task Updated successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction("Task");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, int status)
        {
            await _taskService.ChangeStatusAsync(id, status);
            return RedirectToAction("TaskIndex");
        }

        [HttpPost]
        public async Task<IActionResult> Filter(TaskFilterModel model)
        {
            var tasks = await _taskService.GetFilteredTasksAsync(model);
            ViewData["FilteredCount"] = tasks.Count;
            return PartialView("~/Views/Admin/Task/_List.cshtml", tasks);
        }

        #endregion


        #region User API Calls

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            await _userService.CreateUserAsync(model);
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel model)
        {
            await _userService.UpdateUserAsync(model);
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction("Users");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return PartialView("~/Views/Admin/User/_List.cshtml", users);
        }


        #endregion
    }
}
