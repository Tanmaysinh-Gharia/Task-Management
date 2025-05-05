using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Context;
using TaskManagement.Data.Entities;
namespace TaskManagement.Data.Repositories.RefreshTokenRepo
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveTokenAsync(int userId, RefreshToken token)
        {
            token.UserId = userId;
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetValidTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(
                r => r.Token == token && !r.IsRevoked && r.Expires > DateTime.Now);
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpiredTokensAsync()
        {
            var expiredTokens = _context.RefreshTokens
                .Where(r => r.Expires < DateTime.Now || r.IsRevoked);
            _context.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }
    }
}
