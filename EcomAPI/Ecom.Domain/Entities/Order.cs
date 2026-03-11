using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string Status { get; private set; } = "Pending";
        public string ShippingAddress { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public User User { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
    }
}
