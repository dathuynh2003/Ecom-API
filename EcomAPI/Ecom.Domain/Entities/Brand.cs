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

        public static Brand Create(string name, string? logoUrl, string slug)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Brand name cannot be null or empty.");
            
            return new Brand
            {
                Name = name,
                LogoUrl = logoUrl,
                Slug = slug,
            };
        }

        public void Update(string name, string? logoUrl, string slug)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Brand name cannot be null or empty.");
            
            Name = name;
            LogoUrl = logoUrl;
            Slug = slug;
        }

    }
}
