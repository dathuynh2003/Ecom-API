using Ecom.Domain.Entities;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface IBrandRepository : IGenericRepository<Brand, int>
    {
        Task<bool> IsSlugTakenAsync(string slug, int? excludeId, CancellationToken ct);
        Task<IEnumerable<Brand>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
