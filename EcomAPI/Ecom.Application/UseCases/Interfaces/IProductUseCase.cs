using Ecom.Application.Models.Requests.Product;
using Ecom.Application.Models.Responses.Product;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Interfaces
{
    public interface IProductUseCase
    {
        Task<ApiResponse<ProductResponse>> CreateAsync(CreateProductRequest request, CancellationToken ct = default);
        Task<ApiResponse<ProductResponse>> UpdateAsync(int id, UpdateProductRequest request, CancellationToken ct = default);
        Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken ct = default);
        Task<ApiResponse<PagingResult<ProductResponse>>> GetPagedAsync(PagingRequest request, CancellationToken ct = default);
        Task<ApiResponse<ProductResponse>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ApiResponse<PagingResult<ProductResponse>>> GetByCategorySlugAsync(string categorySlug, PagingRequest request, CancellationToken ct = default);
        Task<ApiResponse<PagingResult<ProductResponse>>> GetByBrandSlugAsync(string brandSlug, PagingRequest request, CancellationToken ct = default);
        Task<ApiResponse<PagingResult<ProductResponse>>> GetByCategoryBrandSlugAsync(string categorySlug, string brandSlug, PagingRequest request, CancellationToken ct = default);
        Task<ApiResponse<PagingResult<ProductResponse>>> GetDetailBySlugAsync(string productSlug, CancellationToken ct = default);
    }
}
