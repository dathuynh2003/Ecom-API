using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class EmailVerificationTokenRepository(AppDbContext context) : GenericRepository<EmailVerificationToken, Guid>(context), IEmailVerificationTokenRepository
    {
        public Task<EmailVerificationToken?> GetByToken(string token)
        {
            return _context.EmailVerificationTokens.FirstOrDefaultAsync(e => e.Token == token);
        }

        public Task<EmailVerificationToken?> GetByUserAsync(Guid userId)
        {
            return _context.EmailVerificationTokens.FirstOrDefaultAsync(e => e.UserId == userId);
        }
    }
}