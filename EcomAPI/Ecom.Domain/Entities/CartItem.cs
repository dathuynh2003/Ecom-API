using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public int Id { get; private set; }
        public Guid CartId { get; private set; }
        public Cart Cart { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public static CartItem Create(Guid cartId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");
            return new()
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity
            };
        }
    }
}
