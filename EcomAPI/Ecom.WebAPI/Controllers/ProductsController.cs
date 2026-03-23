using Ecom.Application.Models.Requests.Product;
using Ecom.Application.Models.Responses.Product;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecom.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductUseCase _useCase;

        public ProductsController(IProductUseCase productUseCase)
        {
            _useCase = productUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ProductResponse>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
        {
            var rs = await _useCase.CreateAsync(request, ct);
            return Ok(rs);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<ProductResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaged([FromQuery] PagingRequest request, CancellationToken ct)
        {
            var rs = await _useCase.GetPagedAsync(request, ct);
            return Ok(rs);
        }

        [HttpGet("detail/{productSlug}")]
        [ProducesResponseType(typeof(ApiResponse<ProductResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailProduct(string productSlug, CancellationToken ct)
        {
            var rs = await _useCase.GetDetailBySlugAsync(productSlug, ct);
            return Ok(rs);
        }

        [HttpGet("by-brand/{brandSlug}")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<ProductResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedByBrandSlug(string brandSlug, [FromQuery] PagingRequest request, CancellationToken ct)
        {
            var rs = await _useCase.GetByBrandSlugAsync(brandSlug, request, ct);
            return Ok(rs);
        }
        [HttpGet("by-category/{categorySlug}")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<ProductResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedByCategorySlug(string categorySlug, [FromQuery] PagingRequest request, CancellationToken ct)
        {
            var rs = await _useCase.GetByCategorySlugAsync(categorySlug, request, ct);
            return Ok(rs);
        }
        [HttpGet("by-category-brand/{categorySlug}/{brandSlug}")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<ProductResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedCategoryBrandSlug(string categorySlug, string brandSlug, [FromQuery] PagingRequest request, CancellationToken ct)
        {
            var rs = await _useCase.GetByCategoryBrandSlugAsync(categorySlug, brandSlug, request, ct);
            return Ok(rs);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductResponse>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request, CancellationToken ct)
        {
            var rs = await _useCase.UpdateAsync(id, request, ct);
            return Ok(rs);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var rs = await _useCase.DeleteAsync(id, ct);
            return Ok(rs);
        }
    }
}
