using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
    }
}
