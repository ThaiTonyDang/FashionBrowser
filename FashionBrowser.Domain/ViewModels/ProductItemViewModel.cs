namespace FashionBrowser.Domain.ViewModels
{
    public class ProductItemViewModel
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Provider { set; get; }
        public string DisplayPrice { set; get; }
        public decimal Price { set; get; }
        public string Description { get; set; }
        public int UnitsInStock { get; set; }
        public bool Enable { get; set; }
        public string Type { get; set; }
        public Guid CategoryId { get; set; }
        public string ImagePath { get; set; }
        public List<CategoryItemViewModel> Categories { get; set; }
        public string CategoryName { get; set; }
        public CategoryItemViewModel Category { get; set; }
        public int QuantityInput { get; set; }
    }
    public class ProductViewModel
    {
        public List<ProductItemViewModel> ListProduct { get; set; }

        public ProductViewModel()
        {
            this.ListProduct = new List<ProductItemViewModel>();
        }
    }
}