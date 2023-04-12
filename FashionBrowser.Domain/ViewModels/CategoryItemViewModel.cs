using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
    public class CategoryItemViewModel
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsCheck { get; set; }       
    }

    public class CategoryViewModel
    {
        public List<ProductItemViewModel> Products { get; set; }
        public List<CategoryItemViewModel> ListCategory { get; set; }
        public CategoryViewModel()
        {
            this.ListCategory = new List<CategoryItemViewModel>();
        }
    }
}