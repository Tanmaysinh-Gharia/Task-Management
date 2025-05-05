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
    public class RefreshTokenSeed : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            //builder.HasData(new RefreshToken
            //{
            //    RefreshTokenId = Guid.NewGuid(),
            //    Token = "sample-refresh-token",
            //    Expires = DateTime.Now.AddDays(7),
            //    CreatedFromIp = "127.0.0.1",
            //    IsRevoked = false,
            //    UserId = 1
            //});
        }
    }

}
