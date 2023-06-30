using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using FashionBrowser.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace Fashion.Browser.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapServices _mapServices;
        public UsersController(IUserService userService, IMapServices mapServices)
        {
            _userService = userService;
            _mapServices = mapServices;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;

            var isAuthentication = User.Identity.IsAuthenticated;
            if (!isAuthentication)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "Login To Shop Now !";
            }

            return await Task.FromResult(View(new LoginItemViewModel()));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginItemViewModel loginItemView, [FromQuery] string returnUrl)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;

            if (loginItemView == null)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "User information cannot be blank ";
                return RedirectToAction("login"); ;
            }

            
            var result = await _userService.LoginAsync(loginItemView);
            if (result.IsSuccess)
            {
                var resultData = result.ToSuccessDataResult<string>();
                var token = resultData.Data;
                var claims = _userService.GetClaims(token);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                });

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                TempData[Mode.LABEL_CONFIRM_SUCCESS] = "Success Login !";
                return RedirectToAction("index", "home");
            }

            TempData[Mode.LABEL_CONFIRM_FAIL] = result.Message;
            return RedirectToAction("login", "users");

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;            
            TempData[Mode.LABEL_CONFIRM_CHECK] = "Register To Shop Now !";
            return await Task.FromResult(View(new RegisterItemViewModel()));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterItemViewModel registerUser)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;

            if (registerUser == null)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "User information cannot be blank ";
                return RedirectToAction("register"); ;
            }

            var message = string.Empty;
            if (ModelState.IsValid)
            {
                await GetAddress(registerUser);

                var result = await _userService.RegisterUserAsync(registerUser);
                message = result.Message;
                if (result.IsSuccess)
                {
                    TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
                    return RedirectToAction("login", "users");
                }
            }    
                       
            TempData[Mode.LABEL_CONFIRM_FAIL] = message;
            return View();
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> Profile()
        {
            var token = User.FindFirstValue(JwtClaimType.TOKEN);
            var user = await _userService.GetUserProfileAsync(token);             
            return await Task.Run(() => View(user));
        }

        [Authorize]
        [HttpPost]
        [Route("profile")]
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

        [Authorize]
        [HttpPost]
        [Route("profile/avatar")]
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

        [Authorize]
        [HttpPost]
        [Route("profile/change-password")]
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

        private async Task GetAddress(RegisterItemViewModel registerUser)
        {
            var cities = await _mapServices.GetCities();
            var cityId = registerUser.CityId;
            var districtId = registerUser.DistrictId;
            var wardId = registerUser.WardId;

            var city = cities.Where(c => c.Id == cityId).FirstOrDefault();
            var district = new District();
            var ward = new Ward();

            if (districtId != null)
            {
                district = city.Districts.Where(d => d.Id == districtId).FirstOrDefault();
            }

            if (wardId != null)
            {
                ward = district.Wards.Where(w => w.Id == wardId).FirstOrDefault();
            }

            registerUser.CityName = city.Name;
            registerUser.DistrictName = district.Name;
            registerUser.WardName = ward.Name;
        }
    }
}
