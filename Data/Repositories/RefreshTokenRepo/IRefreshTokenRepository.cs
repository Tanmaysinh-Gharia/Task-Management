using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Entities;
namespace TaskManagement.Data.Repositories.RefreshTokenRepo
{
    public interface IRefreshTokenRepository
    {
        Task SaveTokenAsync(int userId, RefreshToken token);
        Task<RefreshToken> GetValidTokenAsync(string token);
        Task UpdateAsync(RefreshToken token);
        Task DeleteExpiredTokensAsync();
    }
}
