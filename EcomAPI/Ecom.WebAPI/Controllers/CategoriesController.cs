using Ecom.Application.Models.Requests.Category;
using Ecom.Application.Models.Requests.CategorySpec;
using Ecom.Application.Models.Responses.Brand;
using Ecom.Application.Models.Responses.Category;
using Ecom.Application.Models.Responses.CategorySpec;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecom.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryUseCase _categoryUseCase;
        private readonly ICategorySpecificationKeyUseCase _cateSpecUseCase;
        public CategoriesController(ICategoryUseCase categoryUseCase, ICategorySpecificationKeyUseCase cateSpecUseCase)
        {
            _categoryUseCase = categoryUseCase;
            _cateSpecUseCase = cateSpecUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct)
        {
            var result = await _categoryUseCase.CreateAsync(request, ct);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<CategoryResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaged([FromQuery] PagingRequest request, CancellationToken ct)
        {
            var rs = await _categoryUseCase.GetPagedAsync(request, ct);
            return Ok(rs);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<List<CategoryResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var rs = await _categoryUseCase.GetAllAsync();
            return Ok(rs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var rs = await _categoryUseCase.GetByIdAsync(id, ct);
            return Ok(rs);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
        {
            var rs = await _categoryUseCase.UpdateAsync(id, request, ct);
            return Ok(rs);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var rs = await _categoryUseCase.DeleteAsync(id, ct);
            return Ok(rs);
        }

        [HttpGet("{id}/specification-schema")]
        [ProducesResponseType(typeof(ApiResponse<CategorySpecSchemaResponse>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSpecSchemaById(int id, CancellationToken ct)
        {
            var rs = await _cateSpecUseCase.GetSchemaByCategoryAsync(id, ct);
            return Ok(rs);
        }

        [HttpPut("{id}/specification-schema")]
        [ProducesResponseType(typeof(ApiResponse<CategorySpecSchemaResponse>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSpecSchemaById(int id, [FromBody] UpdateCategorySpecSchemaRequest request, CancellationToken ct)
        {
            var rs = await _cateSpecUseCase.UpdateSchemaAsync(id, request, ct);
            return Ok(rs);
        }
    }
}
