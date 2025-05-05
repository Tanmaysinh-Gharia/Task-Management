using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Entities;

namespace TaskManagement.Data.Configurations
{
    public class TaskDetailConfiguration : IEntityTypeConfiguration<TaskDetail>
    {
        public void Configure(EntityTypeBuilder<TaskDetail> builder)
        {
            builder.Property(t => t.FieldName).IsRequired();
            builder.HasOne(t => t.Task).WithMany(t => t.TaskDetails).HasForeignKey(t => t.TaskId);
            builder.HasOne(t => t.UpdatedBy).WithMany().HasForeignKey(t => t.UpdatedById);
        }
    }

}
