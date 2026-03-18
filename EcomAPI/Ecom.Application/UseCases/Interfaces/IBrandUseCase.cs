using Ecom.Application.Models.Requests.Brand;
using Ecom.Application.Models.Responses.Brand;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Interfaces
{
    public interface IBrandUseCase
    {
        Task<ApiResponse<BrandResponse>> CreateAsync(CreateBrandRequest request, CancellationToken ct = default);
        Task<ApiResponse<PagingResult<BrandResponse>>> GetPagedAsync(PagingRequest request, CancellationToken ct = default);
        Task<ApiResponse<List<BrandResponse>>> GetAllAsync();
        Task<ApiResponse<BrandResponse>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ApiResponse<BrandResponse>> UpdateAsync(int id, UpdateBrandRequest request, CancellationToken ct = default);
        Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
