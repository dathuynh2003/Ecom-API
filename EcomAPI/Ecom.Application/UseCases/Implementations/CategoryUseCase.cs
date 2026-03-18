using Ecom.Application.Abstractions.Persistence;
using Ecom.Application.Models.Requests.Category;
using Ecom.Application.Models.Responses.Category;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Shared.Common;
using Ecom.Shared.Utils;

namespace Ecom.Application.UseCases.Implementations
{
    public class CategoryUseCase : ICategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryUseCase(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<ApiResponse<CategoryResponse>> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default)
        {
            if (request is null)
                return ApiResponse<CategoryResponse>.Fail("Request cannot be null");
            if (string.IsNullOrEmpty(request.Name))
                return ApiResponse<CategoryResponse>.Fail("Invalid data");

            var baseSlug = SlugUtils.GenerateSlug(request.Name);
            var slug = await EnsureUniqueSlugAsync(baseSlug, null, ct);

            var newCate = Category.Create(request.Name, request.Description, slug);

            await _categoryRepo.AddAsync(newCate);
            await _categoryRepo.SaveChangesAsync();

            var response = new CategoryResponse
            {
                Id = newCate.Id,
                Name = newCate.Name,
                Description = newCate.Description,
                Slug = newCate.Slug,
            };

            return ApiResponse<CategoryResponse>.Ok(response, "Category created successfully");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var cate = await _categoryRepo.GetByIdAsync(id);
            if (cate == null)
                return ApiResponse<string>.Fail("Category not found");

            await _categoryRepo.SoftDeleteAsync(cate);
            await _categoryRepo.SaveChangesAsync();
            return ApiResponse<string>.Ok(null, "Category deleted successfully");
        }

        public async Task<ApiResponse<List<CategoryResponse>>> GetAllAsync()
        {
            var items = (await _categoryRepo.GetAllAsync())
                .Where(c => c.IsDeleted == false)
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Slug = c.Slug,
                }).ToList();

            return ApiResponse<List<CategoryResponse>>.Ok(items, "Categories retrieved successfully.");

        }

        public async Task<ApiResponse<CategoryResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var item = await _categoryRepo.GetByIdAsync(id);
            if (item == null)
                return ApiResponse<CategoryResponse>.Fail("Category not found");
            var rs = new CategoryResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Slug = item.Slug,
            };;
            return ApiResponse<CategoryResponse>.Ok(rs, "Category retrieved successfully.");
        }

        public async Task<ApiResponse<PagingResult<CategoryResponse>>> GetPagedAsync(PagingRequest request, CancellationToken ct = default)
        {
            if (request is null)
                return ApiResponse<PagingResult<CategoryResponse>>.Fail("Request cannot be null");
            var pageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            var pageSize = request.PageSize <=0 ? 10 : request.PageSize;

            var items = await _categoryRepo.GetPagedAsync(pageIndex, pageSize, ct);
            var totalItems = await _categoryRepo.CountAsync();
            var mapped = items.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Slug = c.Slug,
            }).ToList();

            var pagingResult = new PagingResult<CategoryResponse>
            {
                Items = mapped,
                TotalItems = totalItems,
                PageNumber = pageIndex,
                PageSize = pageSize,
            };

            return ApiResponse<PagingResult<CategoryResponse>>.Ok(pagingResult, "Categories retrieved successfully.");
        }

        public async Task<ApiResponse<CategoryResponse>> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken ct = default)
        {
            if (request is null)
                return ApiResponse<CategoryResponse>.Fail("Request cannot be null");
            if (string.IsNullOrEmpty(request.Name))
                return ApiResponse<CategoryResponse>.Fail("Invalid data");
            var item = await _categoryRepo.GetByIdAsync(id);
            if (item == null)
                return ApiResponse<CategoryResponse>.Fail("Category not found");
            var baseSlug = SlugUtils.GenerateSlug(request.Name);
            var slug = await EnsureUniqueSlugAsync(baseSlug, id, ct);

            item.Update(request.Name, request.Description, slug);
            await _categoryRepo.UpdateAsync(item);
            await _categoryRepo.SaveChangesAsync();
            var response = new CategoryResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Slug = item.Slug,
            };
            return ApiResponse<CategoryResponse>.Ok(response, "Category updated successfully");
        }

        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, int? excludeId, CancellationToken ct)
        {
            var slug = baseSlug;
            var suffix = 1;

            while (await _categoryRepo.IsSlugTakenAsync(slug, excludeId, ct))
            {
                slug = $"{baseSlug}-{suffix++}";
            }

            return slug;
        }
    }
}
