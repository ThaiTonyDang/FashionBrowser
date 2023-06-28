using FashionBrowser.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
    public class CategoryItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsCheck { get; set; }
        public string Slug { get; set; }
        public ICollection<CategoryItemViewModel> CategoryChildrens { get; set; }
        public ICollection<ProductItemViewModel> ProductDtos { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public CategoryItemViewModel ParentCategory { get; set; }
    }

    public class CategoryViewModel
    {
        public List<CategoryItemViewModel> ListCategory { get; set; }
        public string[] ErrorDetail;
        public bool IsSuccess;
        public CategoryViewModel()
        {
            this.ListCategory = new List<CategoryItemViewModel>();
        }
    }
}