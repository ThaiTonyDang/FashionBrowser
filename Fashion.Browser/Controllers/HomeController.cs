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

        [HttpGet]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var productViewModel = await _productServices.GetPagingProductViewModel(currentPage);
           
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
        public async Task<IActionResult> ProductsCategory(string categoryName, int currentPage = 1)
        {                       
            var category = await _categoryServices.GetCategoryByName(categoryName);

            var productViewModel = await _categoryServices.GetPagingProductViewByNameAsync(categoryName, currentPage);
            productViewModel.CategoryItem = category;       
            return View(productViewModel);         
        }

        [HttpGet]           
        [Route("categories")]
        public async Task<IActionResult> ProductsCategoryChildren([FromQuery] string childSlug, int currentPage = 1)
        {
            var category = await _categoryServices.GetCategoryChildrenBySlug(childSlug);
            var productViewModel = await _categoryServices.GetPagingProductViewBySlugAsync(childSlug, currentPage);
            productViewModel.CategoryItem = category;

            return View(productViewModel);
        }
    }
}
