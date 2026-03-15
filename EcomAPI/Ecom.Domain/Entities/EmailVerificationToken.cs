using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class EmailVerificationToken : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; } = false;
        public User User { get; private set; } = default!;

        public static EmailVerificationToken Create(Guid userId, string token, DateTime expiresAt)
        {
            return new EmailVerificationToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAt,
            };
        }

        public void Refresh(string newToken, DateTime newExpiresTime)
        {
            Token = newToken;
            ExpiresAt = newExpiresTime;
        }

        public void MarkAsUsed() => IsUsed = true;
    }
}
