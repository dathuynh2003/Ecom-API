using Ecom.Application.Models.Requests.Brand;
using Ecom.Application.Models.Requests.Category;
using Ecom.Application.Models.Responses.Brand;
using Ecom.Application.Models.Responses.Category;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Interfaces
{
    public interface ICategoryUseCase
    {
        Task<ApiResponse<CategoryResponse>> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default);
        Task<ApiResponse<PagingResult<CategoryResponse>>> GetPagedAsync(PagingRequest request, CancellationToken ct = default);
        Task<ApiResponse<List<CategoryResponse>>> GetAllAsync();
        Task<ApiResponse<CategoryResponse>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ApiResponse<CategoryResponse>> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken ct = default);
        Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
