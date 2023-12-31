﻿using FashionBrowser.Domain.Dto;
using System.Globalization;

namespace FashionBrowser.Domain.ViewModels
{
    public class ProductItemViewModel
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Provider { set; get; }
        public string DisplayPrice { get; set; }
        public decimal Price { set; get; }
        public string Description { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public string MainImageName { get; set; }
        public string CategoryName { get; set; }
        public int QuantityInput { get; set; }
    }
    public class ProductViewModel
    {
        public List<ProductItemViewModel> ListProduct { get; set; }
        public List<ProductItemViewModel> ListProductCategory { get; set; }
        public CategoryItemViewModel CategoryItem { get; set; }
        public Paging Paging { get; set; }
        public bool IsSuccess { get; set; }
        public string[] ErrorDetail { get; set; }
        public string CateName { get; set; }

        public ProductViewModel()
        {
            this.ListProduct = new List<ProductItemViewModel>();
        }
    }
}