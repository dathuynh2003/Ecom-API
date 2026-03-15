using Ecom.Domain.Entities;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface IEmailVerificationTokenRepository : IGenericRepository<EmailVerificationToken, Guid>
    {
        public Task<EmailVerificationToken?> GetByToken(string token);
        public Task<EmailVerificationToken?> GetByUserAsync(Guid userId);
    }
}
