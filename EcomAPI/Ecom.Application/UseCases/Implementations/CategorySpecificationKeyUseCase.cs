using Ecom.Application.Abstractions.Persistence;
using Ecom.Application.Models.Requests.CategorySpec;
using Ecom.Application.Models.Responses.CategorySpec;
using Ecom.Application.Models.Responses.CategorySpecKey;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Implementations
{
    public class CategorySpecificationKeyUseCase : ICategorySpecificationKeyUseCase
    {
        private readonly ICategoryRepository _cateRepo;
        public CategorySpecificationKeyUseCase(ICategoryRepository cateRepo)
        {
            _cateRepo = cateRepo;
        }
        public async Task<ApiResponse<CategorySpecSchemaResponse>> GetSchemaByCategoryAsync(int categoryId, CancellationToken ct = default)
        {
            var category = await _cateRepo.GetWithSpecKeyByIdAsync(categoryId, ct);
            if (category is null)
                return ApiResponse<CategorySpecSchemaResponse>.Fail("Category not found");
            var schemaItems = category.CategorySpecificationKeys.Select(x => new CategorySpecKeyItemResponse
            {
                SpecificationKeyId = x.SpecificationKeyId,
                Name = x.SpecificationKey.Name ?? string.Empty,
                Unit = x.SpecificationKey.Unit,
                DisplayOrder = x.DisplayOrder
            }).ToList();

            var response = new CategorySpecSchemaResponse
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                Items = schemaItems
            };

            return ApiResponse<CategorySpecSchemaResponse>
                .Ok(response, "Category specification schema retrieved successfully");
        }

        public async Task<ApiResponse<CategorySpecSchemaResponse>> UpdateSchemaAsync(int categoryId, UpdateCategorySpecSchemaRequest request, CancellationToken ct = default)
        {
            var category = await _cateRepo.GetWithSpecKeyByIdAsync(categoryId, ct);
            if (category is null)
                return ApiResponse<CategorySpecSchemaResponse>.Fail("Category not found");

            var curCateSpecs = category.CategorySpecificationKeys;

            var requestIds = request.Items.Select(x => x.SpecificationKeyId).ToHashSet();

            // Delete những spec key không còn trong request
            var toDelList = curCateSpecs.Where(x => !requestIds.Contains(x.SpecificationKeyId)).ToList();
            foreach (var delItem in toDelList)
            {
                curCateSpecs.Remove(delItem);
            }

            // Update display order và thêm mới
            foreach (var item in request.Items)
            {
                var existing = curCateSpecs.FirstOrDefault(c => c.SpecificationKeyId == item.SpecificationKeyId);
                if (existing is null) //  Thêm mới
                {
                    var newCateSpec = CategorySpecificationKey.Create(categoryId, item.SpecificationKeyId, item.DisplayOrder);
                    curCateSpecs.Add(newCateSpec);
                }
                else // Update display order
                {
                    existing.UpdateDisplayOrder(item.DisplayOrder);
                }
            }

            // Update xuống DB
            await _cateRepo.UpdateAsync(category);
            await _cateRepo.SaveChangesAsync();

            // Mapped về Reponse mới
            // Lấy lại category specs sau khi đã update để đảm bảo dữ liệu mới nhất
            var updatedSchemaItems = curCateSpecs.Select(x => new CategorySpecKeyItemResponse
            {
                SpecificationKeyId = x.SpecificationKeyId,
                Name = x.SpecificationKey.Name ?? string.Empty,
                Unit = x.SpecificationKey.Unit,
                DisplayOrder = x.DisplayOrder
            }).ToList();
            var response = new CategorySpecSchemaResponse
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                Items = updatedSchemaItems
            };
            return ApiResponse<CategorySpecSchemaResponse>
                .Ok(response, "Category specification schema updated successfully");
        }
    }
}
