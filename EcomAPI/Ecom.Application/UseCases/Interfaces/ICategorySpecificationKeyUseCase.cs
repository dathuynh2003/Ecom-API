using Ecom.Application.Models.Requests.CategorySpec;
using Ecom.Application.Models.Responses.CategorySpec;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Interfaces
{
    public interface ICategorySpecificationKeyUseCase
    {
        Task<ApiResponse<CategorySpecSchemaResponse>> GetSchemaByCategoryAsync(int categoryId, CancellationToken ct = default);
        Task<ApiResponse<CategorySpecSchemaResponse>> UpdateSchemaAsync(int categoryId, UpdateCategorySpecSchemaRequest request, CancellationToken ct = default);
    }
}
