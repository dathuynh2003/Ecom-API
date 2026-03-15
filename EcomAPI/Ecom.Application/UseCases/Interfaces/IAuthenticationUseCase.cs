using Ecom.Application.Models.Requests.Auth;
using Ecom.Application.Models.Responses.Auth;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Interfaces
{
    public interface IAuthenticationUseCase
    {
        Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<TokenResponse>> GetNewAccessToken(TokenRequest request);
        Task<ApiResponse<string>> LogoutAsync(Guid userId);
        Task<ApiResponse<string>> VerifyEmailAsync(string token);
        Task<ApiResponse<string>> ResendVerificationEmailAsync(string email);
    }
}
