using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Core.InjectionInterfaces;
using TaskManagement.Services.LoginServices;
using TaskManagement.Services.TaskServices;
using TaskManagement.Services.UserServices;

namespace TaskManagement.Services
{
    public class DependencyInjection : IDependencyInjection
    {
        /// <summary>
        /// Registers all service layer dependencies, including login, user, and task services,
        /// along with the centralized response handler.
        /// </summary>
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IResponseHandler, ResponseHandler>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();

        }

        /// <summary>
        /// Specifies the order of registration for the service layer (value = 2).
        /// </summary>
        public int Order => 2;
    }
}
