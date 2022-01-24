namespace MicroMarket.Models
{
    public class Transaction
    {
        public virtual Guid TransactionId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual DateTime TransactionTime { get; set; }
    }
}