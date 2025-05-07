using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Bussiness.Authentication;
using TaskManagement.Bussiness.TaskManage;
using TaskManagement.Bussiness.UserManage;
using TaskManagement.Core.Common.Authentication.Background;
using TaskManagement.Core.InjectionInterfaces;
namespace TaskManagement.Bussiness
{
    public class DependencyInjection : IDependencyInjection
    {
        #region Public Methods
        /// <summary>
        /// Registers all business layer dependencies including managers for auth, user, and task.
        /// Called during application startup.
        /// </summary>
        public void Register(
            IServiceCollection serviceCollection, 
            IConfiguration configuration)
        {
            #region OnRequestJobs
            AddAuthentication(serviceCollection);
            AddUserManager(serviceCollection);
            AddTaskManager(serviceCollection);
            #endregion

            #region Background Jobs

            //AddRefreshTokenCleanupService(serviceDescriptors, configuration);
            #endregion
        }

        /// <summary>
        /// Gets the execution order for dependency registration.
        /// </summary>
        public int Order => 1;
        #endregion

        #region Private Methods
        /// <summary>
        /// Registers the authentication manager service (IAuthManager → AuthManager).
        /// </summary>
        private static void AddAuthentication(IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
        }
        /// <summary>
        /// Configures and registers the refresh token cleanup background service (currently commented).
        /// </summary>
        private static void AddRefreshTokenCleanupService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RefreshTokenCleanupSettings>(configuration.GetSection("BackgroundService:CleanUp:RefreshToken"));

            //services.AddHostedService<RefreshTokenCleanupService>();
        }

        /// <summary>
        /// Registers the user manager service (IUserManager → UserManager).
        /// </summary>
        private static void AddUserManager(IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();
        }

        /// <summary>
        /// Registers the task manager service (ITaskManager → TaskManager).
        /// </summary>
        private static void AddTaskManager(IServiceCollection services)
        {
            services.AddScoped<ITaskManager, TaskManager>();
        }
        #endregion
    }
}
