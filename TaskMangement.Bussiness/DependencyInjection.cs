using Microsoft.AspNetCore.RequestDecompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Bussiness.Authentication;
using TaskManagement.Bussiness.UserManage;
using TaskManagement.Core.Common.Authentication.Background;
using TaskManagement.Core.InjectionInterfaces;
namespace TaskManagement.Bussiness
{
    public class DependencyInjection : IDependencyInjection
    {
        #region Public Methods
        public void Register(
            IServiceCollection serviceCollection, 
            IConfiguration configuration)
        {
            #region OnRequestJobs
            AddAuthentication(serviceCollection);
            AddUserManager(serviceCollection);
            #endregion

            #region Background Jobs

            //AddRefreshTokenCleanupService(serviceDescriptors, configuration);
            #endregion
            // Register your services here
            // Example: serviceCollection.AddScoped<IMyService, MyService>();
        }
        
        public int Order => 1;
        #endregion

        #region Private Methods

        private static void AddAuthentication(IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
        }

        private static void AddRefreshTokenCleanupService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RefreshTokenCleanupSettings>(configuration.GetSection("BackgroundService:CleanUp:RefreshToken"));

            //services.AddHostedService<RefreshTokenCleanupService>();
        }

        private static void AddUserManager(IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();
        }
        #endregion
    }
}
