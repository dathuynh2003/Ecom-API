using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public int Id { get; private set; }
        public string ImageUrl { get; private set; }
        public bool IsPrimary { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }
        public static ProductImage Create(string url, bool isPrimary)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL required");
            return new()
            {
                ImageUrl = url,
                IsPrimary = isPrimary
            };
        }
    }
}
