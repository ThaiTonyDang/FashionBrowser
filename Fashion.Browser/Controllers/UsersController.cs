using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

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

        public IActionResult Login()
        {
            var loginUser = new LoginItemViewModel();
            return View(loginUser);
        }

        [HttpPost]
        [Route("users/login")]
        public async Task<IActionResult> Login(LoginItemViewModel loginItemView, [FromQuery] string returnUrl)
        {
            var message = "";
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
    
            if (loginItemView == null)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "User information cannot be blank ";
                return RedirectToAction("login", "users"); ;
            }

            var result = await _userService.LoginAsync(loginItemView);
            var claimsPrincipal = result.Item1;
            var isSuccess = result.Item2;
            var content = result.Item3;

            if (isSuccess)
            {
                var token = content;
                var claims = _userService.GetClaims(claimsPrincipal, token);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = loginItemView.RememberLogin
                });


                if (!string.IsNullOrEmpty(returnUrl))
                {
                    TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
                    return Redirect(returnUrl);
                }

                TempData[Mode.LABEL_CONFIRM_SUCCESS] = "Success Login !";
                return RedirectToAction("index", "home");
            }

            TempData[Mode.LABEL_CONFIRM_FAIL] = content;
            return RedirectToAction("index", "home");

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
