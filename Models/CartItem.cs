namespace MicroMarket.Models
{
    public class CartItem
    {
        public CartItem(Product _Product)
        {
            Product = _Product;
            Quantity = 1;
        }

        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
