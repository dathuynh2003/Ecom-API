using Ecom.Domain.Entities;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        Task<bool> IsSlugTakenAsync(string slug, int? excludeId, CancellationToken ct = default);
        Task<IEnumerable<Product>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken ct = default);
        Task<IEnumerable<Product>> GetPagedByCategorySlugAsync(string categorySlug, int pageIndex, int pageSize, CancellationToken ct = default);
        Task<int> CountByCategorySlugAsync(string categorySlug, CancellationToken ct = default);
        Task<IEnumerable<Product>> GetPagedByBrandSlugAsync(string brandSlug, int pageIndex, int pageSize, CancellationToken ct = default);
        Task<int> CountByBrandSlugAsync(string brandSlug, CancellationToken ct = default);
        Task<IEnumerable<Product>> GetPagedByCategoryBrandSlugAsync(string categorySlug, string brandSlug, int pageIndex, int pageSize, CancellationToken ct = default);
        Task<int> CountByCategoryBrandSlugAsync(string categorySlug, string brandSlug, CancellationToken ct = default);
        Task<Product?> GetDetailBySlugAsync(string productSlug, CancellationToken ct = default);
    }
}
