using Ecom.Application.Abstractions.Persistence;
using Ecom.Application.Models.Requests.Brand;
using Ecom.Application.Models.Responses.Brand;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Shared.Common;
using Ecom.Shared.Utils;

namespace Ecom.Application.UseCases.Implementations
{
    public class BrandUseCase : IBrandUseCase
    {
        private readonly IBrandRepository _brandRepo;

        public BrandUseCase(IBrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public async Task<ApiResponse<BrandResponse>> CreateAsync(CreateBrandRequest request, CancellationToken ct = default)
        {
            if (request is null)
                return ApiResponse<BrandResponse>.Fail("Request cannot be null.");
            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<BrandResponse>.Fail("Invalid data");

            var baseSlug = SlugUtils.GenerateSlug(request.Name);
            var slug = await EnsureUniqueSlugAsync(baseSlug, null, ct);

            var brand = Brand.Create(request.Name, request.LogoUrl, slug);

            await _brandRepo.AddAsync(brand);
            await _brandRepo.SaveChangesAsync();

            // Map to response
            var response = new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = brand.LogoUrl,
                Slug = brand.Slug
            };

            return ApiResponse<BrandResponse>.Ok(response, "Brand created successfully.");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var brand = await _brandRepo.GetByIdAsync(id);

            if (brand is null)
                return ApiResponse<string>.Fail("Brand not found.");

            await _brandRepo.SoftDeleteAsync(brand);
            await _brandRepo.SaveChangesAsync();
            return ApiResponse<string>.Ok(null, "Brand deleted successfully.");
        }

        public async Task<ApiResponse<List<BrandResponse>>> GetAllAsync()
        {
            var brands = (await _brandRepo.GetAllAsync())
                .Where(b => !b.IsDeleted)
                .Select(brand => new BrandResponse
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    LogoUrl = brand.LogoUrl,
                    Slug = brand.Slug
                }).ToList();
            return ApiResponse<List<BrandResponse>>.Ok(brands, "Brands retrieved successfully.");
        }

        public async Task<ApiResponse<BrandResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var brand = await _brandRepo.GetByIdAsync(id);

            if (brand is null)
                return ApiResponse<BrandResponse>.Fail("Brand not found.");

            var response = new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = brand.LogoUrl,
                Slug = brand.Slug
            };

            return ApiResponse<BrandResponse>.Ok(response, "Brand retrieved successfully.");
        }

        public async Task<ApiResponse<PagingResult<BrandResponse>>> GetPagedAsync(PagingRequest request, CancellationToken ct = default)
        {
            var pageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var items = await _brandRepo.GetPagedAsync(pageIndex, pageSize, ct);
            var totalCount = await _brandRepo.CountAsync();

            var mapped = items.Select(brand => new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = brand.LogoUrl,
                Slug = brand.Slug
            }).ToList();

            var pagingResult = new PagingResult<BrandResponse>
            {
                Items = mapped,
                TotalItems = totalCount,
                PageNumber = pageIndex,
                PageSize = pageSize
            };

            return ApiResponse<PagingResult<BrandResponse>>.Ok(pagingResult, "Brands retrieved successfully.");
        }

        public async Task<ApiResponse<BrandResponse>> UpdateAsync(int id, UpdateBrandRequest request, CancellationToken ct = default)
        {
            if (request is null)
                return ApiResponse<BrandResponse>.Fail("Request cannot be null.");
            if (string.IsNullOrEmpty(request.Name))
                return ApiResponse<BrandResponse>.Fail("Invalid data");
            var brand = await _brandRepo.GetByIdAsync(id);
            if (brand is null)
                return ApiResponse<BrandResponse>.Fail("Brand not found.");
            var baseSlug = SlugUtils.GenerateSlug(request.Name);
            var slug = await EnsureUniqueSlugAsync(baseSlug, id, ct);

            brand.Update(request.Name, request.LogoUrl, slug);
            return ApiResponse<BrandResponse>.Ok(new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = brand.LogoUrl,
                Slug = brand.Slug
            }, "Brand updated successfully.");
        }
        /// <summary>
        /// Generates a unique slug based on the specified base slug, appending a numeric suffix if necessary to avoid
        /// conflicts.
        /// </summary>
        /// <remarks>This method is typically used when creating or updating entities to ensure that their
        /// slug is unique within the repository. If the base slug is already taken, a numeric suffix is appended and
        /// incremented until a unique slug is found.</remarks>
        /// <param name="baseSlug">The initial slug value to use as the base for uniqueness checks. Cannot be null or empty.</param>
        /// <param name="excludeId">An optional identifier to exclude from the uniqueness check. If specified, the slug associated with this ID
        /// will not be considered a conflict. Must be null if Create</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A unique slug string that does not conflict with existing slugs, based on the provided base slug.</returns>
        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, int? excludeId, CancellationToken ct)
        {
            var slug = baseSlug;
            var suffix = 1;

            while (await _brandRepo.IsSlugTakenAsync(slug, excludeId, ct))
            {
                slug = $"{baseSlug}-{suffix++}";
            }

            return slug;
        }
    }
}
