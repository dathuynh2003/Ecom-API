using Ecom.Application.Models.Requests.SpecificationKey;
using Ecom.Application.Models.Responses.SpecificationKey;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Interfaces
{
    public interface ISpecificationKeyUseCase
    {
        Task<ApiResponse<SpecKeyResponse>> CreateAsync(CreateSpecKeyRequest request);
        Task<ApiResponse<SpecKeyResponse>> UpdateAsync(int id, UpdateSpecKeyRequest request);
        Task<ApiResponse<string>> DeleteAsync(int id);
        Task<ApiResponse<SpecKeyResponse>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<SpecKeyResponse>>> GetAllAsync();
    }
}
