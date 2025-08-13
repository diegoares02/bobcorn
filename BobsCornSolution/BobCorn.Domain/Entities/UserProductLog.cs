namespace BobsCorn.Domain.Entities
{
    public class UserProductLog
    {
        public int UserProductLogId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool Status { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
