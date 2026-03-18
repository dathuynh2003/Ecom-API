namespace Ecom.Application.Models.Responses.Product
{
    public class ProductImageItemResponse
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}
