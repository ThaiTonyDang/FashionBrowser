using Microsoft.AspNetCore.Mvc;

namespace Fashion.Browser.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
