using Ecom.Application.Abstractions.Auth;
using Ecom.Application.Abstractions.Persistence;
using Ecom.Application.Models.Requests.Auth;
using Ecom.Application.Models.Responses.Auth;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Implementations
{
    public class AuthenticationUseCase : IAuthenticationUseCase
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;

        public AuthenticationUseCase(IUserRepository userRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Requests a new access token using the specified token request parameters.
        /// </summary>
        /// <param name="request">The parameters required to request a new access token. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ApiResponse with the token
        /// response data if the request is successful.</returns>
        public async Task<ApiResponse<TokenResponse>> GetNewAccessToken(TokenRequest request)
        {
            var isValid = await _jwtService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (isValid == false)
            {
                return ApiResponse<TokenResponse>.Fail("Invalid refresh token!");
            }

            var user = await _userRepo.GetByIdAsync(request.UserId);
            if (user == null)
                return ApiResponse<TokenResponse>.Fail("User not found.");

            var reponseToken = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.ToString()),
                RefreshToken = request.RefreshToken
            };
            return ApiResponse<TokenResponse>.Ok(reponseToken, "New access token generated successfully.");
        }

        /// <summary>
        /// Authenticates a user asynchronously using the specified login request and returns a token response.
        /// </summary>
        /// <param name="request">The login request containing user credentials and any additional authentication parameters. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ApiResponse with the token
        /// response if authentication is successful.</returns>
        public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
        {
            // Validate the login request parameters
            if (request == null)
                return ApiResponse<TokenResponse>.Fail("Login request cannot be null.");
            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
                return ApiResponse<TokenResponse>.Fail("Account and password must be provided.");

            var user = await _userRepo.LoginAsync(request.Account, request.Password);
            if (user == null)
                return ApiResponse<TokenResponse>.Fail("Invalid account or password.");

            var tokenResponse = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.ToString()),
                RefreshToken = await _jwtService.GenerateRefreshToken(user.Id)
            };
            return ApiResponse<TokenResponse>.Ok(tokenResponse, "Login successful.");
        }

        /// <summary>
        /// Logs out the specified user by revoking their refresh token asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to log out.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ApiResponse indicating the
        /// outcome of the logout operation. If successful, the response contains a success message; otherwise, it
        /// contains an error message.</returns>
        public async Task<ApiResponse<string>> LogoutAsync(Guid userId)
        {
            var result = await _jwtService.RevokeRefreshTokenAsync(userId);
            if (result == false)
                return ApiResponse<string>.Fail("Logout failed or user already logged out");
            return ApiResponse<string>.Ok(null, "Logout successful");
        }

        public Task<ApiResponse<string>> RegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
