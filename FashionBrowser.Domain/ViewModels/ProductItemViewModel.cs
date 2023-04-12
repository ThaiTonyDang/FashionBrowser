using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        public int QuantityInput { get; set; } = 1;
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