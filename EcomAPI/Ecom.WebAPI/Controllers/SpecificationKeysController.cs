using Ecom.Application.Models.Requests.SpecificationKey;
using Ecom.Application.Models.Responses.SpecificationKey;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecom.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SpecificationKeysController : ControllerBase
    {
        private readonly ISpecificationKeyUseCase _specificationKeyUseCase;
        public SpecificationKeysController(ISpecificationKeyUseCase specificationKeyUseCase)
        {
            _specificationKeyUseCase = specificationKeyUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SpecKeyResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateSpecKeyRequest request)
        {
            var result = await _specificationKeyUseCase.CreateAsync(request);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SpecKeyResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _specificationKeyUseCase.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<SpecKeyResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _specificationKeyUseCase.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<SpecKeyResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecKeyRequest request)
        {
            var result = await _specificationKeyUseCase.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _specificationKeyUseCase.DeleteAsync(id);
            return Ok(result);
        }
    }
}
