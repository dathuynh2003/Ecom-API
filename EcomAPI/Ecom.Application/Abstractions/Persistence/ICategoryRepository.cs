using Ecom.Domain.Entities;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category, int>
    {
        Task<bool> IsSlugTakenAsync(string slug, int? excludeId, CancellationToken ct);
        Task<IEnumerable<Category>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<Category?> GetWithSpecKeyByIdAsync(int id, CancellationToken ct = default);
    }
}
