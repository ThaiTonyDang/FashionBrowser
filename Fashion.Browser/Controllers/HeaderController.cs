using Microsoft.AspNetCore.Mvc;

namespace Fashion.Browser.Controllers
{
    public class HeaderController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
