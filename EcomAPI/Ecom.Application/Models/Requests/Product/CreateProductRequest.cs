namespace Ecom.Application.Models.Requests.Product
{
    public class CreateProductRequest
    {
        // Core 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }

        // Images 
        public List<ProductImageRequest> Images { get; set; }
        
        // Specs
        public List<ProductSpecificationRequest> Specifications { get; set; }
    }
}
