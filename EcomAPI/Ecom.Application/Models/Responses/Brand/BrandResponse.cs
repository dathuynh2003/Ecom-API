namespace Ecom.Application.Models.Responses.Brand
{
    public class BrandResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? LogoUrl { get; init; }
        public string Slug { get; init; } = string.Empty;
    }
}
