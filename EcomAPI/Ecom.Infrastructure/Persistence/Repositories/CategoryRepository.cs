using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category, int>(context), ICategoryRepository
    {
        public async Task<IEnumerable<Category>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            // Calculate the number of items to skip based on the page number and page size
            var skip = (pageNumber - 1) * pageSize;
            // Retrieve the paged list of brands, ordered by name
            return await _context.Categories
                .AsNoTracking()
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<Category?> GetWithSpecKeyByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Categories
                .Include(c => c.CategorySpecificationKeys)
                .ThenInclude(csk => csk.SpecificationKey)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
        }

        public async Task<bool> IsSlugTakenAsync(string slug, int? excludeId, CancellationToken ct)
        {
            return await _context.Categories.AnyAsync(b => b.Slug == slug && (!excludeId.HasValue || b.Id != excludeId.Value), ct);
        }
    }
}
