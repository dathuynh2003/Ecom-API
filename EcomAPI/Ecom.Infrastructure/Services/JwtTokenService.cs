using Ecom.Application.Abstractions.Auth;
using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ecom.Infrastructure.Services
{
    public class JwtTokenService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IGenericRepository<RefreshToken, Guid> _repository;

        /// <summary>
        /// JWT Service constructor that initializes the JWT settings and the repository for managing refresh tokens.
        /// </summary>
        /// <param name="jwtSettings"></param>
        /// <param name="repository"></param>
        public JwtTokenService(IOptions<JwtSettings> jwtSettings, IGenericRepository<RefreshToken, Guid> repository)
        {
            _jwtSettings = jwtSettings.Value;
            _repository = repository;
        }

        /// <summary>
        /// Generates a JWT access token for the specified user ID and role. 
        /// The token will include claims for the user's identity and role, and will be signed using a secret key. 
        /// The token will have an expiration time to ensure it is only valid for a limited period.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GenerateAccessToken(Guid userId, string role)
        {
            var claims = new Claim[]
            {
                new (ClaimTypes.NameIdentifier, userId.ToString()),
                new (ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates and stores a new refresh token for the specified user, replacing any existing refresh token
        /// associated with the user.
        /// </summary>
        /// <remarks>If the user already has an existing refresh token, it is replaced with the new token.
        /// Otherwise, a new refresh token entry is created for the user. The refresh token's expiration is determined
        /// by the configured refresh token expiry period.</remarks>
        /// <param name="userId">The unique identifier of the user for whom the refresh token is generated.</param>
        /// <returns>A base64-encoded string representing the newly generated refresh token.</returns>
        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
            var existingToken = (await _repository.FindAsync(rt => rt.UserId == userId)).FirstOrDefault();

            if (existingToken != null)
            {
                // Update the existing token
                existingToken.Update(refreshToken, expiry);
                await _repository.UpdateAsync(existingToken);
            }
            else
            {
                // Create a new token
                var newTokenEntity = RefreshToken.Create(userId, refreshToken, expiry);
                _ = await _repository.AddAsync(newTokenEntity);
            }
            await _repository.SaveChangesAsync();

            return refreshToken;
        }

        /// <summary>
        /// Gets the user ID associated with the specified JWT token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Guid> GetUserIdAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return Guid.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateLifetime = false,
            }, out SecurityToken validatedToken);

            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Guid.Empty;

            return userId;
        }

        /// <summary>
        /// Revokes (deletes) the refresh token associated with the specified user, if one exists.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose refresh token is to be revoked.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if a refresh
        /// token was found and revoked; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> RevokeRefreshTokenAsync(Guid userId)
        {
            var existingToken = (await _repository.FindAsync(rt => rt.UserId == userId)).FirstOrDefault();
            if (existingToken == null)
                return true;

            await _repository.HardDeleteAsync(existingToken);
            await _repository.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Validates the provided refresh token for the specified user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var tokenEntity = (await _repository.FindAsync(rt => rt.UserId == userId && rt.Token == refreshToken)).FirstOrDefault();
            return tokenEntity != null &&
                tokenEntity.UserId == userId &&
                tokenEntity.ExpiresAt > DateTime.UtcNow;
        }
    }
}
