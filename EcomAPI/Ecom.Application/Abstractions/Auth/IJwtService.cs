namespace Ecom.Application.Abstractions.Auth
{
    public interface IJwtService
    {
        string GenerateAccessToken(Guid userId, string role);
        Task<string> GenerateRefreshToken(Guid userId);
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
        Task<Guid> GetUserIdAsync(string token);
        Task<bool> RevokeRefreshTokenAsync(Guid userId);
    }
}
