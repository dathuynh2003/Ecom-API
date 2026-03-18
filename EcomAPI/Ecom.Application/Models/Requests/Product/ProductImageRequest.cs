namespace Ecom.Application.Models.Requests.Product
{
    public class ProductImageRequest
    {
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
    }
}
