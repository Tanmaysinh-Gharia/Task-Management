using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.InjectionInterfaces;
using TaskManagement.Services.BaseServices;
using TaskManagement.Services.LoginServices;
using TaskManagement.Services.TaskServices;
using TaskManagement.Services.UserServices;

namespace TaskManagement.Services
{
    public class DependencyInjection : IDependencyInjection
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IResponseHandler, ResponseHandler>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();

        }
        public int Order => 2;
    }
}
