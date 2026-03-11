using Ecom.Application.Models.Requests.Auth;
using Ecom.Application.Models.Responses.Auth;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Ecom.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthenticationUseCase _authUseCase;

        public AuthenticationsController(IAuthenticationUseCase authUseCase)
        {
            _authUseCase = authUseCase;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authUseCase.LoginAsync(request);
            if (result.Success)
                return Ok(result);
            return Unauthorized(result);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken(TokenRequest request)
        {
            var rs = await _authUseCase.GetNewAccessToken(request);
            if (rs.Success) return Ok(rs);
            return BadRequest(rs);
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized(ApiResponse<string>.Fail("Invalid token"));
            var result = await _authUseCase.LogoutAsync(userId);

            if (result.Success)
                return Ok(result);
            return BadRequest(result);

        }
    }
}
