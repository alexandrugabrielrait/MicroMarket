namespace MicroMarket.Models
{
    public class TransactionProduct
    {
        public virtual Guid TransactionProductId { get; set; }
        public virtual Guid TransactionId { get; set; }
        public virtual int ProductId { get; set; }
        public virtual int Quantity { get; set; }

        public TransactionProduct() { }

        public TransactionProduct(Guid transactionId, CartItem cartItem)
        {
            TransactionProductId = Guid.NewGuid();
            TransactionId = transactionId;
            ProductId = cartItem.Product.ProductId;
            Quantity = cartItem.Quantity;
        }
    }
}