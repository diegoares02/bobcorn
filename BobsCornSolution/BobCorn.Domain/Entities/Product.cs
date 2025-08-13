namespace BobsCorn.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; } = 0;
        public IEnumerable<UserProductLog> UserProductLogs { get; set; }
    }
}
