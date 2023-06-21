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
using System.Linq;
using FashionBrowser.Domain.Model;
using System;

namespace Fashion.Browser.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Login()
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var claim = User.FindFirst("token");
            if (claim == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "Login To Shop Now !";
            }
            var loginUser = new LoginItemViewModel();
            return await Task.Run(() => View(loginUser));
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
            return RedirectToAction("login", "users");

        }

        [HttpGet]
        [Route("users/register")]
        public async Task<IActionResult> Register()
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var user = new UserItemViewModel();
            TempData[Mode.LABEL_CONFIRM_CHECK] = "Register To Shop Now !";
            return await Task.Run(() => View(user));
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
                    return RedirectToAction("login", "users");
                }
            }

            TempData[Mode.LABEL_CONFIRM_FAIL] = message;
            return View();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        [Route("users/profile")]
        public async Task<IActionResult> Profile()
        {
            var token = User.FindFirst("token").Value;
            var user = await _userService.GetUserAsync(token);             
            return await Task.Run(() => View(user));
        }

        [HttpPost]
        [Route("users/profile")]
        public async Task<IActionResult> Profile(UserItemViewModel userItemViewModel)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var email = User.FindFirstValue(ClaimTypes.Email);
            var token = User.FindFirst("token").Value;
            if (userItemViewModel == null)
            {
                return View(new UserItemViewModel());
            }
            userItemViewModel.Email = email;
            var result = await _userService.UpdateUserAsync(userItemViewModel, token);
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (!isSuccess)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = message;
                return View(new UserItemViewModel());
            }

            TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
            return RedirectToAction("Profile", "Users");
        }

        
        [HttpPost]
        [Route("users/avatar-update")]
        public async Task<IActionResult> Avatar(UserItemViewModel userItemViewModel)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var token = User.FindFirst("token").Value;
            if (userItemViewModel == null)
            {
                return RedirectToAction("Profile", "Users");
            }

            var result = await _userService.UpdateUserAvatarAsync(userItemViewModel, token);
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (!isSuccess)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = message;
                return RedirectToAction("Profile", "Users");
            }

            TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
            return RedirectToAction("Profile", "Users");
        }

        [HttpPost]
        [Route("users/change-password")]
        public async Task<IActionResult> ChangePassword(UserItemViewModel userItemViewModel)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var token = User.FindFirst("token").Value;
            if (userItemViewModel == null)
            {
                return RedirectToAction("Profile", "Users");
            }
            var paswordItem = userItemViewModel.PasswordItemViewModel;
            var result = await _userService.ChangePassword(paswordItem, token);
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (!isSuccess)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = message;
                return RedirectToAction("Profile", "Users");
            }

            TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
            return RedirectToAction("Profile", "Users");
        }

    }
}
