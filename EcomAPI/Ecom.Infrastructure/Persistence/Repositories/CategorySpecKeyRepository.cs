using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class CategorySpecKeyRepository(AppDbContext context) : GenericRepository<CategorySpecificationKey, int>(context), ICategorySpecKeyRepository
    {
        public async Task<IEnumerable<CategorySpecificationKey>> GetByCategoryIdAsync(int categoryId, CancellationToken ct = default)
        {
            return await _context.CategorySpecificationKeys
                .Where(csk => csk.CategoryId == categoryId && !csk.IsDeleted)
                .Include(csk => csk.SpecificationKey)
                .OrderBy(csk => csk.DisplayOrder)
                .ToListAsync(ct);
        }
    }
}
