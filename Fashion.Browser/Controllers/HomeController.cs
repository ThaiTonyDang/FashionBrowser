using FashionBrowser.Domain.Services;
using FashionBrowser.Utilities;
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
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
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

            if (User.FindFirst("token") == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "Welcome to S: Shop! Please Login Now To Shopping ";
            }
            return View(productViewModel);
        }

        [Route("{productId}")]
        public async Task<IActionResult> Detail(string productId)
        {
            var result = productId.IsGuidParseFromString();
            if (result)
            {
                var tuple= await _productServices.GetProductByIdAsync(productId);
                var product = tuple.Item1;
                return View(product);
            }    
            return RedirectToAction("Index", "Home");
        }
    }
}
