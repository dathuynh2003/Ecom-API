using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? LogoUrl { get; private set; } = string.Empty;
        public string Slug { get; private set; }
        public ICollection<Product> Products { get; private set; }
    }
}
