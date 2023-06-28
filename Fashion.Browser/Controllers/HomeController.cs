using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                }
            }

            if (User.FindFirst("token") == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "Welcome to S: Shop! Please Login Now To Shopping ";
            }
            return View(productViewModel);
        }

        [Route("product-detail/{productId}")]
        public async Task<IActionResult> Detail(string productId)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var result = productId.IsGuidParseFromString();
            if (result)
            {
                var tuple= await _productServices.GetProductByIdAsync(productId);
                var product = tuple.Item1;
                return View(product);
            }    
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("products/{categoryName}")]
        public async Task<IActionResult> ProductsCategory(string categoryName)
        {
            var productViewModel = new ProductViewModel();
            var categoryViewModel = await _categoryServices.GetCategoryViewModelAsync();
            var categories = categoryViewModel.ListCategory;

            var products = await _categoryServices.GetAllProductsByCategoryName(categories, categoryName);
            var category = await _categoryServices.GetCategoryByName(categories, categoryName);

            productViewModel.CategoryItem = category;
            productViewModel.ListProductCategory = products;
            
            return View(productViewModel);         
        }

        [HttpGet]           
        [Route("categories")]
        public async Task<IActionResult> ProductsCategoryChildren([FromQuery] string childSlug)
        {
            var productViewModel = new ProductViewModel();
            var categoryViewModel = await _categoryServices.GetCategoryViewModelAsync();
            var categories = categoryViewModel.ListCategory;

            var result = _categoryServices.GetProductCategoryChildren(categories, childSlug);
            productViewModel.ListProductCategory = result.Item1;
            productViewModel.CategoryItem = result.Item2;

            return View(productViewModel);
        }
    }
}
