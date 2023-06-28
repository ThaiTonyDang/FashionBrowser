using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fashion.Browser.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        public CategoriesController(IProductServices productServices,
                             ICategoryServices categoryServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/{childSlug}")]
        public async Task<IActionResult> ProductsCategoryChildren(string childSlug)
        {
            var productViewModel = new ProductViewModel();
            var categoryViewModel = await _categoryServices.GetCategoryViewModelAsync();
            var categories = categoryViewModel.ListCategory;

            var result = _categoryServices.GetProductCategoryChildren(categories, childSlug);
            productViewModel.ListProduct = result.Item1;
            productViewModel.CategoryItem = result.Item2;
            if (productViewModel.CategoryItem != null && productViewModel.ListProduct != null)
            {
                productViewModel.IsSuccess = true;
            }

            return View(productViewModel);
        }
    }
}
