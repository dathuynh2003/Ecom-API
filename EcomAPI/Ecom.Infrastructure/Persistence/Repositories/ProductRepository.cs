using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class ProductRepository(AppDbContext context) : GenericRepository<Product, int>(context), IProductRepository
    {
        public async Task<int> CountByBrandSlugAsync(string brandSlug, CancellationToken ct = default)
        {
            return await _context.Products
                .Where(p => p.Brand.Slug.Equals(brandSlug) && !p.IsDeleted)
                .CountAsync(ct);
        }

        public async Task<int> CountByCategoryBrandSlugAsync(string categorySlug, string brandSlug, CancellationToken ct = default)
        {
            return await _context.Products
                .Where(p => p.Brand.Slug.Equals(brandSlug) && p.Category.Slug.Equals(categorySlug) && !p.IsDeleted)
                .CountAsync(ct);
        }

        public async Task<int> CountByCategorySlugAsync(string categorySlug, CancellationToken ct = default)
        {
            return await _context.Products
                .Where(p => p.Category.Slug.Equals(categorySlug) && !p.IsDeleted)
                .CountAsync(ct);
        }

        public async Task<Product?> GetDetailBySlugAsync(string productSlug, CancellationToken ct = default)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.ProductSpecificationKeys)
                    .ThenInclude(psk => psk.SpecificationKey)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Slug.Equals(productSlug) && p.IsDeleted == false, ct);
        }

        public async Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking()
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Product>> GetPagedByBrandSlugAsync(string brandSlug, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var skip = (pageIndex - 1) * pageSize;
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.Brand.Slug.Equals(brandSlug) && p.IsDeleted == false)
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Product>> GetPagedByCategoryBrandSlugAsync(string categorySlug, string brandSlug, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var skip = (pageIndex - 1) * pageSize;
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Where(p => p.Category.Slug.Equals(categorySlug) && p.Brand.Slug.Equals(brandSlug) && p.IsDeleted == false)
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Product>> GetPagedByCategorySlugAsync(string categorySlug, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var skip = (pageIndex - 1) * pageSize;
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Where(p => p.Category.Slug.Equals(categorySlug) && p.IsDeleted == false)
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Asynchronously determines whether a product with the specified slug exists, optionally excluding a product
        /// by its identifier.
        /// excludeId must be null if Create and must have a value if Update. 
        /// This method is useful for ensuring that product slugs are unique when creating or updating products.
        /// </summary>
        /// <param name="slug">The slug to check for uniqueness among products. Cannot be null.</param>
        /// <param name="excludeId">The identifier of a product to exclude from the check, or null to include all products.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if a
        /// product with the specified slug exists (excluding the specified product if provided); otherwise, <see
        /// langword="false"/>.</returns>
        public async Task<bool> IsSlugTakenAsync(string slug, int? excludeId, CancellationToken ct = default)
        {
            return await _context.Products.AnyAsync(b => b.Slug.Equals(slug) && (!excludeId.HasValue || b.Id != excludeId.Value), ct);
        }
    }
}
