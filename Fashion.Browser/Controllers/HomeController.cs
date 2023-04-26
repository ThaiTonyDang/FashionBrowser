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
        private readonly ILogger<HomeController> _logger;
        private readonly IProductServices _productServices;
        public HomeController(ILogger<HomeController> logger, IProductServices productServices)
        {
            _logger = logger;
            _productServices = productServices;
        }

        public async Task<IActionResult> Index()
        {
            var productViewModel = await _productServices.GetProductViewModelAsync();
            return View(productViewModel);
        }
    }
}
