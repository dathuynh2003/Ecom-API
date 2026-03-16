using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class EmailVerificationTokenRepository(AppDbContext context) : GenericRepository<UserToken, Guid>(context), IEmailVerificationTokenRepository
    {
        public async Task<UserToken?> GetByToken(string token)
        {
            return await _context.UserTokens.FirstOrDefaultAsync(e => e.Token == token);
        }

        public async Task<UserToken?> GetByUserAsync(Guid userId)
        {
            return await _context.UserTokens.FirstOrDefaultAsync(e => e.UserId == userId);
        }
    }
}