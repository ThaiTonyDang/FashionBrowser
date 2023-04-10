namespace FashionBrowser.Infrastructure.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Freight { get; set; }
        public string Status { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
