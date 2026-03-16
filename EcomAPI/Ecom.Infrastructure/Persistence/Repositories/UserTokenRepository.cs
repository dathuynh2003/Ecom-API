using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class UserTokenRepository(AppDbContext context) : GenericRepository<UserToken, Guid>(context), IUserTokenRepository
    {
        public async Task<UserToken?> GetByToken(string token)
        {
            return await _context.UserTokens.FirstOrDefaultAsync(e => e.Token == token);
        }

        public async Task<UserToken?> GetByUserAsync(Guid userId)
        {
            return await _context.UserTokens.FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<UserToken?> GetValidTokenByToken(string Token)
        {
            return await _context.UserTokens.FirstOrDefaultAsync(e => e.Token == Token && !e.IsUsed && e.ExpiresAt > DateTime.UtcNow);
        }
    }
}