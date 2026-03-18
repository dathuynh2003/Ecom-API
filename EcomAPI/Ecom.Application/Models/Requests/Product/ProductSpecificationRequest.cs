namespace Ecom.Application.Models.Requests.Product
{
    public class ProductSpecificationRequest
    {
        public int SpecificationKeyId { get; set; }
        public string? Value { get; set; } = string.Empty;
    }
}
