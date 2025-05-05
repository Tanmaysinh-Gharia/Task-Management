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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id).ValueGeneratedOnAdd(); // Ensures auto-increment
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).IsRequired();
            builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.HasMany(u => u.RefreshTokens)
                   .WithOne(rt => rt.User)
                   .HasForeignKey(rt => rt.UserId);

            builder.HasMany(u => u.CreatedTasks)
                   .WithOne(t => t.Creator)
                   .HasForeignKey(t => t.CreatorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.AssignedTasks)
                   .WithOne(t => t.Assignee)
                   .HasForeignKey(t => t.AssigneeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
