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

        public static Category Create(string name, string? description, string slug)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be null or empty.");
            
            return new Category
            {
                Name = name,
                Description = description,
                Slug = slug,
            };
        }

        public void Update(string name, string? description, string slug)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be null or empty.");
            
            Name = name;
            Description = description;
            Slug = slug;
        }
    }
}
