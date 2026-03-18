using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class SpecificationKeyRepository(AppDbContext context) : GenericRepository<SpecificationKey, int>(context), ISpecificationKeyRepository
    {
        public async Task<IEnumerable<SpecificationKey>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await _context.SpecificationKeys
                .AsNoTracking()
                .Where(sk => !sk.IsDeleted)
                .Skip(skip)
                .OrderBy(sk => sk.Id)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
