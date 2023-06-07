using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> Login(LoginItemViewModel loginItemView)
        {
            var message = "";
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            if (ModelState.IsValid)
            {
                if (loginItemView == null)
                {
                    TempData[Mode.LABEL_CONFIRM_FAIL] = "User information cannot be blank ";
                    return RedirectToAction("login", "users"); ;
                }

                var result = await _userService.VerifyUserAsync(loginItemView);
                var principal = result.Item1;
                var isSuccess = result.Item2;
                var content = result.Item3;

                if (!isSuccess)
                {
                    message = content;
                    TempData[Mode.LABEL_CONFIRM_FAIL] = message;
                    return RedirectToAction("login", "users");
                }

                var token = content;
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = loginItemView.RememberLogin
                });

                HttpContext.Session.SetString("JwtToken", content);
                TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
                return RedirectToAction("index", "home");
                
            }

            TempData[Mode.LABEL_CONFIRM_FAIL] = "Some Fields Need Not Yet Entered";
            return RedirectToAction("login", "users");
        }

        [HttpPost]
        [Route("users/register")]
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
