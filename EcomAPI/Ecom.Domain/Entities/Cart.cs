using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid? UserId { get; private set; }
        public User? User { get; private set; }
        public string SessionID { get; private set; } = string.Empty;  // For guest carts
        public ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

        public static Cart CreateForUser(Guid userId) => new Cart { UserId = userId };
        public static Cart CreateForGuest(string sessionId) => new Cart { SessionID = sessionId };
    }
}
