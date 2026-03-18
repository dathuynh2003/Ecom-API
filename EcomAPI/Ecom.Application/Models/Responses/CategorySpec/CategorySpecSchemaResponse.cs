using Ecom.Application.Models.Responses.CategorySpecKey;

namespace Ecom.Application.Models.Responses.CategorySpec
{
    public class CategorySpecSchemaResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public IEnumerable<CategorySpecKeyItemResponse> Items { get; set; } = [];
    }
}
