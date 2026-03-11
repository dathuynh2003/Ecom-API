using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Token { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public Guid UserId { get; private set; }
        public User User { get; private set; }  // Nullable + virtual EF

        public static RefreshToken Create(Guid userId, string token, DateTime expiresAt)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token required");
            return new()
            {
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAt
            };
        }

        public void Update(string newToken, DateTime newExpiry)
        {
            if (string.IsNullOrWhiteSpace(newToken))
                throw new ArgumentException("New token required");
            Token = newToken;
            ExpiresAt = newExpiry;
        }
    }
}
