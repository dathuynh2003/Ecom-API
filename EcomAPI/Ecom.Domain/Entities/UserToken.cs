using Ecom.Domain.Base;
using Ecom.Domain.Enums;

namespace Ecom.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; } = false;
        public UserTokenType Type { get; private set; }
        public User User { get; private set; } = default!;

        public static UserToken Create(Guid userId, string token, DateTime expiresAt, UserTokenType type)
        {
            return new UserToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAt,
                Type = type,
            };
        }

        public void Refresh(string newToken, DateTime newExpiresTime)
        {
            Token = newToken;
            ExpiresAt = newExpiresTime;
            IsUsed = false;
        }

        public void MarkAsUsed() => IsUsed = true;
    }
}
