using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Core.Enums;
using TaskManagement.Data.Entities;
using TaskStatus = TaskManagement.Core.Enums.TaskStatus;

namespace TaskManagement.Data.Seed
{
    public class TaskSeed : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasData(new TaskEntity
            {
                Id = 1,
                Title = "Initial Task",
                Description = "This is a seeded task for demo purposes.",
                Status = TaskStatus.Todo,
                Priority = TaskPriority.Medium,
                DueDate = new DateTime(2025, 5, 10, 1, 1, 1, DateTimeKind.Local),
                CreatedAt = new DateTime(2025, 5, 4, 1, 1, 1, DateTimeKind.Local),
                UpdatedAt = new DateTime(2025, 5, 5, 1, 1, 1, DateTimeKind.Local),
                CreatorId = 1,
                AssigneeId = 1,
                IsDeleted = false
            });
        }
    }

}
