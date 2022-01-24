namespace MicroMarket.Models
{
    public class Product
    {
        public virtual int ProductId { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
        public virtual int ProductTypeId { get; set; }
        public virtual string? Description { get; set; }
        public virtual int Stock { get; set; }
        public virtual string? ImageSource { get; set; }
    }
}