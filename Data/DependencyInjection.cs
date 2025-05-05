using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.InjectionInterfaces;
using TaskManagement.Data.Context;
using TaskManagement.Data.Repositories.RefreshTokenRepo;
using TaskManagement.Data.Repositories.TaskRepo;
using TaskManagement.Data.Repositories.UserRepo;

namespace TaskManagement.Data
{
    public class DependencyInjection : IDependencyInjection
    {
        #region Public Methods
        public virtual void Register(
            IServiceCollection serviceCollection, 
            IConfiguration configuration)
        {
            // Add DbContext
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            #region Inject Repositories
            AddUserRepository(serviceCollection);

            AddRefreshTokenRepository(serviceCollection);

            AddTaskRepository(serviceCollection);

            AddTransaction(serviceCollection);
            #endregion
        }

        public int Order => 3;

        #endregion

        #region Private Methods
        private static void AddUserRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }
        private static void AddRefreshTokenRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        }
        private static void AddTaskRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITaskRepository, TaskRepository>();
        }
        private static void AddTransaction(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        #endregion
    }
}
