using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        /// <summary>
        /// Registers database context and repository dependencies for the data layer.
        /// </summary>
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

        /// <summary>
        /// Specifies the registration order for the data layer (value = 3).
        /// </summary>
        public int Order => 3;

        #endregion

        #region Private Methods
        /// <summary>
        /// Registers the user repository interface and implementation.
        /// </summary>
        private static void AddUserRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }

        /// <summary>
        /// Registers the refresh token repository interface and implementation.
        /// </summary>
        private static void AddRefreshTokenRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        }
        /// <summary>
        /// Registers the task repository interface and implementation.
        /// </summary>
        private static void AddTaskRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITaskRepository, TaskRepository>();
        }
        /// <summary>
        /// Registers the unit of work interface and implementation for transaction support.
        /// </summary>
        private static void AddTransaction(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        #endregion
    }
}
