using Ecom.Domain.Entities;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface ICategorySpecKeyRepository : IGenericRepository<CategorySpecificationKey, int>
    {
        Task<IEnumerable<CategorySpecificationKey>> GetByCategoryIdAsync(int categoryId, CancellationToken ct = default);
    }
}
