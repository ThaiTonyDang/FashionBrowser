using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Fashion.Browser.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("users/register")]
        public IActionResult Register()
        {
            var user = new UserItemViewModel();
            return View(user);
        }

        [Route("users/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("users/login")]
        public async Task<IActionResult> Register(UserItemViewModel registerUser)
        {
            var message = "";
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            if (ModelState.IsValid)
            {
                if (registerUser == null)
                {
                    TempData[Mode.LABEL_CONFIRM_FAIL] = "User information cannot be blank ";
                    return RedirectToAction("register", "users"); ;
                }

                var result = await _userService.RegisterUserAsync(registerUser);
                var isSuccess = result.Item1;
                message = result.Item2;

                if (isSuccess)
                {
                    TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
                    return RedirectToAction("login","users");
                }             
            }

            TempData[Mode.LABEL_CONFIRM_FAIL] = message;
            return View();
        }
    }
}
