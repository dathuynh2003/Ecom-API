using Ecom.Application.Abstractions.Persistence;
using Ecom.Application.Models.Requests.Product;
using Ecom.Application.Models.Responses.Product;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Shared.Common;
using Ecom.Shared.Utils;

namespace Ecom.Application.UseCases.Implementations
{
    public class ProductUseCase : IProductUseCase
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly ICategorySpecKeyRepository _cateSpecRepo;

        public ProductUseCase(IProductRepository productRepo, ICategoryRepository categoryRepo, IBrandRepository brandRepo, ICategorySpecKeyRepository cateSpecRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _brandRepo = brandRepo;
            _cateSpecRepo = cateSpecRepo;
        }

        public async Task<ApiResponse<ProductResponse>> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
        {
            if (request is null)
                return ApiResponse<ProductResponse>.Fail("Request cannot be null.");
            if (string.IsNullOrWhiteSpace(request.Name)
                 || request.Price <= 0
                 || request.StockQuantity < 0)
                return ApiResponse<ProductResponse>.Fail("Invalid Data");
            var category = await _categoryRepo.GetByIdAsync(request.CategoryId);
            if (category is null)
                return ApiResponse<ProductResponse>.Fail("Category not found.");
            var brand = await _brandRepo.GetByIdAsync(request.BrandId);
            if (brand is null)
                return ApiResponse<ProductResponse>.Fail("Brand not found.");

            // Schema Specs for Product From Category
            var schema = await _cateSpecRepo.GetByCategoryIdAsync(request.CategoryId);
            var allowedKeyIds = schema.Select(x => x.SpecificationKeyId).ToHashSet();
            var specKeysDict = schema.Select(x => x.SpecificationKey).ToDictionary(k => k.Id);

            var baseSlug = SlugUtils.GenerateSlug(request.Name);
            var slug = await EnsureUniqueSlugAsync(baseSlug, null, ct);

            var product = Product.Create(request.Name, request.Description, request.Price, request.StockQuantity, request.CategoryId, request.BrandId, slug);

            // Images
            foreach (var img in request.Images)
            {
                if (string.IsNullOrEmpty(img.ImageUrl))
                    return ApiResponse<ProductResponse>.Fail("Invalid specification key for this category.");

                var image = ProductImage.Create(img.ImageUrl, img.IsPrimary);
                product.Images.Add(image);
            }

            //Specs
            foreach (var spec in request.Specifications)
            {
                if (!allowedKeyIds.Contains(spec.SpecificationKeyId)) continue;
                var entity = ProductSpecificationKey.Create(spec.SpecificationKeyId, spec.Value);
                product.ProductSpecificationKeys.Add(entity);
            }

            await _productRepo.AddAsync(product);
            await _productRepo.SaveChangesAsync();

            var pImagesRes = product.Images.Select(i => new ProductImageItemResponse
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                IsPrimary = i.IsPrimary
            }).ToList();

            var pSpecsRes = product.ProductSpecificationKeys.Select(ps =>
            {
                specKeysDict.TryGetValue(ps.SpecificationKeyId, out var specKey);
                return new ProductSpecificationItemResponse
                {
                    SpecificationKeyId = ps.SpecificationKeyId,
                    Name = specKey?.Name ?? string.Empty,
                    Unit = specKey?.Unit,
                    Value = ps.Value
                };
            }).ToList();


            var rs = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Slug = product.Slug,
                CategoryId = category.Id,
                CategoryName = category.Name,
                CategorySlug = category.Slug,
                BrandId = brand.Id,
                BrandName = brand.Name,
                BrandSlug = brand.Slug,

                //Images
                Images = pImagesRes,
                //Specs
                Specifications = pSpecsRes
            };

            return ApiResponse<ProductResponse>.Ok(rs, "Product created successfully.");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null)
                return ApiResponse<string>.Fail("Product not found.");

            await _productRepo.SoftDeleteAsync(product);
            return ApiResponse<string>.Ok(null, "Product deleted successfully.");
        }

        public Task<ApiResponse<PagingResult<ProductResponse>>> GetByBrandSlugAsync(string brandSlug, PagingRequest request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<PagingResult<ProductResponse>>> GetByCategoryBrandSlugAsync(string categorySlug, string brandSlug, PagingRequest request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<PagingResult<ProductResponse>>> GetByCategorySlugAsync(string categorySlug, PagingRequest request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<ProductResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            //var item = await _productRepo.GetByIdAsync(id);
            //if (item is null)
            //    return ApiResponse<ProductResponse>.Fail("Product not found.");
            //var category = await _categoryRepo.GetByIdAsync(item.CategoryId);
            //var brand = await _brandRepo.GetByIdAsync(item.BrandId);
            throw new NotImplementedException();
        }

        public Task<ApiResponse<PagingResult<ProductResponse>>> GetDetailBySlugAsync(string productSlug, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<PagingResult<ProductResponse>>> GetPagedAsync(PagingRequest request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<ProductResponse>> UpdateAsync(int id, UpdateProductRequest request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, int? excludeId, CancellationToken ct)
        {
            var slug = baseSlug;
            var suffix = 1;

            while (await _productRepo.IsSlugTakenAsync(slug, excludeId, ct))
            {
                slug = $"{baseSlug}-{suffix++}";
            }

            return slug;
        }
    }
}
