using Ecom.Domain.Entities;
using System.Linq.Expressions;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface ISpecificationKeyRepository : IGenericRepository<SpecificationKey, int>
    {
        Task<IEnumerable<SpecificationKey>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
