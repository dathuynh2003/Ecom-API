using Ecom.Domain.Entities;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface IEmailVerificationTokenRepository : IGenericRepository<UserToken, Guid>
    {
        public Task<UserToken?> GetByToken(string token);
        public Task<UserToken?> GetByUserAsync(Guid userId);
    }
}
