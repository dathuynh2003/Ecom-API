namespace Ecom.Application.Models.Responses.Product
{
    public class ProductResponse
    {
        // Core
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Slug { get; set; } = string.Empty;
        // Category
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategorySlug { get; set; } = string.Empty;
        // Brand
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandSlug { get; set; } = string.Empty;
        // Images
        public List<ProductImageItemResponse> Images { get; set; }
        // Specs
        public List<ProductSpecificationItemResponse> Specifications { get; set; }
    }
}
