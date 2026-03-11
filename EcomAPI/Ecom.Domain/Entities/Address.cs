using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class Address : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public string AddressText { get; private set; } = string.Empty;
        public bool IsDefault { get; private set; } = false;
        public User User { get; private set; }  // Nullable + virtual EF

        public static Address Create(Guid userId, string addressText)
        {
            if (string.IsNullOrWhiteSpace(addressText))
                throw new ArgumentException("Address required");

            return new()
            {
                UserId = userId,
                AddressText = addressText
            };
        }
    }
}
