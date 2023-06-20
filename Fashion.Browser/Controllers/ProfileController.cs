using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Fashion.Browser.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var user = new UserItemViewModel
            {
                FirstName = User.Claims.FirstOrDefault(x => x.Type == "firstName")?.Value,
                LastName = User.Claims.FirstOrDefault(x => x.Type == "lastName")?.Value,
                Email = User.FindFirstValue(ClaimTypes.Email),
                PhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone),
                AvailableAddress = User.FindFirstValue(ClaimTypes.StreetAddress),
            };
            // load dữ liệu User lên đây
            return View(user);
        }

        
    }
}
