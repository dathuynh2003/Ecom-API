using Ecom.Domain.Entities;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface IUserTokenRepository : IGenericRepository<UserToken, Guid>
    {
        public Task<UserToken?> GetByToken(string token);
        public Task<UserToken?> GetValidTokenByToken(string Token);
        public Task<UserToken?> GetByUserAsync(Guid userId);
    }
}
