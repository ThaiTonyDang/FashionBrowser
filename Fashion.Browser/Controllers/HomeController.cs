using FashionBrowser.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Fashion.Browser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        public HomeController(IProductServices productServices,
                             ICategoryServices categoryServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
        }

        public async Task<IActionResult> Index()
        {
            var productViewModel = await _productServices.GetProductViewModelAsync();
            var categoryViewModel = await _categoryServices.GetCategoryViewModelAsync();
            var listCategory = categoryViewModel.ListCategory;
            if (productViewModel.IsSuccess)
            {
                foreach (var productItemViewModel in productViewModel.ListProduct)
                {
                    if (listCategory == null)
                    {
                        productItemViewModel.CategoryName = "FEMALE FASHION";
                        break;
                    }
                    var category = listCategory.Where(c => c.Id == productItemViewModel.CategoryId).FirstOrDefault();
                    productItemViewModel.CategoryName = category.Name;
                }
            }
            return View(productViewModel);
        }
    }
}
