using Microsoft.AspNetCore.Mvc;

namespace Fashion.Browser.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
