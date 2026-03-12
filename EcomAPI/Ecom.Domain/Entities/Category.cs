using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class Category : BaseEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string Slug { get; private set; }
        public ICollection<Product> Products { get; private set; }
        public ICollection<CategorySpecificationKey> CategorySpecificationKeys { get; private set; }
    }
}
