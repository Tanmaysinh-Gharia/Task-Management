using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Entities;

namespace TaskManagement.Data.Seed
{
    public class TaskDetailSeed : IEntityTypeConfiguration<TaskDetail>
    {
        public void Configure(EntityTypeBuilder<TaskDetail> builder)
        {
            builder.HasData(new TaskDetail
            {
                Id = 1,
                TaskId = 1,
                UpdatedById = 1,
                FieldName = "Status",
                OldValue = null,
                NewValue = "Todo",
                ChangeTime = new DateTime(2025, 5, 4, 1, 1, 1, DateTimeKind.Local)
            });
        }
    }

}
