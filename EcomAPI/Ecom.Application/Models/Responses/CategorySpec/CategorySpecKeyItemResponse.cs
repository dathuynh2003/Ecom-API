namespace Ecom.Application.Models.Responses.CategorySpecKey
{
    public class CategorySpecKeyItemResponse
    {
        public int SpecificationKeyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Unit { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
