using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public int CategoryId { get; private set; }
        public int BrandId { get; private set; }
        public Category Category { get; private set; }
        public Brand Brand { get; private set; }
        public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();
        public static Product Create(string name, string description, decimal price, int stockQuantity, int categoryId, int brandId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required");
            if (price < 0)
                throw new ArgumentException("Price cannot be negative");
            if (stockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative");
            return new()
            {
                Name = name,
                Description = description,
                Price = price,
                StockQuantity = stockQuantity,
                CategoryId = categoryId,
                BrandId = brandId
            };
        }
    }
}
