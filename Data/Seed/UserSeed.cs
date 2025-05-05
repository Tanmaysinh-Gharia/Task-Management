using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Enums;
using TaskManagement.Data.Entities;
using TaskManagement.Core.Helpers;

namespace TaskManagement.Data.Seed
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        private readonly Hashing _hashing;

        public UserSeed(Hashing hashing)
        {
            _hashing = hashing;
        }
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                Id = 1,
                Email = "admin@gmail.com",
                UserName = "Light Admin",
                FirstName = "Admin",
                LastName = "Lost",
                PhoneNumber = "1234567891",
                PasswordHash = "$2a$11$YHcJvjxbP78hEJROwJ4VeOQ51M4LHFbqK9p8cW/HqD66lUlzEVSRi", // Use real hash in production
                Role = Role.Admin,
                CreatedAt = new DateTime(2025, 5, 4, 1, 1, 1, DateTimeKind.Local),
                IsActive = true
            },
            new User { 
                Id = 2,
                Email = "tanmay@gmail.com",
                UserName = "Tanmay",
                FirstName = "Tanmaysinh",
                LastName = "Gharia",
                PhoneNumber = "1234567890",
                PasswordHash = "$2a$11$2EO8SJK1eZy6KBLz3h9OuOzskDcoOjsQv5H80YYbRtjUIMiYO3N4C", // Use real hash in production
                Role = Role.User,
                CreatedAt = new DateTime(2025, 5, 3, 1, 1, 1, DateTimeKind.Local),
                IsActive = true
            });
        }
    }

}
