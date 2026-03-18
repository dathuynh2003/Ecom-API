namespace Ecom.Application.Models.Requests.CategorySpec
{
    public class UpdateCategorySpecSchemaRequest
    {
        public List<CategorySpecKeyUpdateItem> Items { get; set; } = new();
    }
}
