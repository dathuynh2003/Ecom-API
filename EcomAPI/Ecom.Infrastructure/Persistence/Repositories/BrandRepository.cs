using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class BrandRepository(AppDbContext context) : GenericRepository<Brand, int>(context), IBrandRepository
    {
        public async Task<IEnumerable<Brand>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            // Calculate the number of items to skip based on the page number and page size
            var skip = (pageNumber - 1) * pageSize;
            // Retrieve the paged list of brands, ordered by name
            return await _context.Brands
                .AsNoTracking()
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<bool> IsSlugTakenAsync(string slug, int? excludeId, CancellationToken ct)
        {
            // Check if any brand has the same slug, excluding the brand with the specified ID (if provided)
            // Example: If excludeId is 5, it will check for any brand with the same slug but ignore the brand with ID 5.
            return await _context.Brands.AnyAsync(b => b.Slug == slug && (!excludeId.HasValue || b.Id != excludeId.Value), ct);
        }
    }
}
