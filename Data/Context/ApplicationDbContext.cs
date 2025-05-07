using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.ViewModels.TaskManagement;
using TaskManagement.Data.Configurations;
using TaskManagement.Data.Entities;
using TaskManagement.Data.Seed;
namespace TaskManagement.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly Hashing _hashing;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,Hashing hashing) : base(options)
        {
            _hashing = hashing;
        }

        // Creating Tables
        public DbSet<User> Users { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskDetail> TaskDetails { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Creating Store Procedure (SP)
        public DbSet<TaskListItemViewModel> TaskListItemViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region ApplyConfigurations
            
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TaskEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TaskDetailConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            
            #endregion

            #region Seeding

            modelBuilder.ApplyConfiguration(new UserSeed(_hashing));
            modelBuilder.ApplyConfiguration(new TaskSeed());
            modelBuilder.ApplyConfiguration(new TaskDetailSeed());
            modelBuilder.ApplyConfiguration(new RefreshTokenSeed());

            #endregion

            #region Stored Procedures

            modelBuilder.Entity<TaskListItemViewModel>().HasNoKey().ToView(null);
            
            #endregion
        }
    }
}
