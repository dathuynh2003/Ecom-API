using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid OrderId { get; private set; }
        public string Provider { get; private set; } = "PayOS";
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "VND";
        public string Status { get; private set; } = "Pending";
        public string TransactionCode { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; private set; }
        public Order Order { get; private set; }

        public static Payment Create(Guid orderId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");
            return new Payment { OrderId = orderId, Amount = amount };
        }

    }
}
