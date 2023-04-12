namespace FashionBrowser.Infrastructure.Models
{
    public class Product
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Provider { set; get; }
        public decimal Price { set; get; }
        public string Description { get; set; }
        public int UnitsInStock { get; set; }
        public string ImagePath { get; set; }
        public bool Enable { get; set; }
        public string Type { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
