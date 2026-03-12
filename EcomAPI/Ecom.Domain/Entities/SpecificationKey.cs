using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class SpecificationKey : BaseEntity
    {
        public int Id { get; private set; }
        public string? Name { get; private set; } = null;
        public string? Unit { get; private set; } = string.Empty;
        public ICollection<CategorySpecificationKey> CategorySpecificationKeys { get; private set; } = new List<CategorySpecificationKey>();
        public ICollection<ProductSpecificationKey> ProductSpecificationKeys { get; private set; } = new List<ProductSpecificationKey>();
    }
}
