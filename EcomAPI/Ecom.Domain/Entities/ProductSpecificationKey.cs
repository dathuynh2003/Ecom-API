using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class ProductSpecificationKey : BaseEntity
    {
        // composite key (ProductId, SpecificationKeyId)
        public int ProductId { get; private set; }
        public int SpecificationKeyId { get; private set; }
        public string? Value { get; private set; } = string.Empty;
        public Product Product { get; private set; }
        public SpecificationKey SpecificationKey { get; private set; }
    }
}
