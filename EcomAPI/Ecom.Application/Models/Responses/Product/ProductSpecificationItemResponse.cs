namespace Ecom.Application.Models.Responses.Product
{
    public class ProductSpecificationItemResponse
    {
        public int SpecificationKeyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Unit { get; set; } = string.Empty;
        public string? Value { get; set; } = string.Empty;
    }
}
