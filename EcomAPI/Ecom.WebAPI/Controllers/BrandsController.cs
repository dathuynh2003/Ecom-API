using Ecom.Application.Models.Requests.Brand;
using Ecom.Application.Models.Responses.Brand;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecom.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandUseCase _brandUseCase;

        public BrandsController(IBrandUseCase brandUseCase)
        {
            _brandUseCase = brandUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BrandResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateBrandRequest request, CancellationToken ct)
        {
            var result = await _brandUseCase.CreateAsync(request, ct);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<BrandResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaged([FromQuery] PagingRequest request, CancellationToken ct)
        {
            var rs = await _brandUseCase.GetPagedAsync(request, ct);
            return Ok(rs);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<List<BrandResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBrands()
        {
            var rs = await _brandUseCase.GetAllAsync();
            return Ok(rs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BrandResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var rs = await _brandUseCase.GetByIdAsync(id, ct);
            return Ok(rs);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BrandResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandRequest request, CancellationToken ct)
        {
            var rs = await _brandUseCase.UpdateAsync(id, request, ct);
            return Ok(rs);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var rs = await _brandUseCase.DeleteAsync(id, ct);
            return Ok(rs);
        }
    }
}
