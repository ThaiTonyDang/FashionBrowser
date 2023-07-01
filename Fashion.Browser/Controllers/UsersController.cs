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
using FashionBrowser.Domain.Model.Users;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using FashionBrowser.Domain.ViewModels.Users;

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

            return await Task.FromResult(View(new UserLoginViewModel()));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginViewModel loginItemView, [FromQuery] string returnUrl)
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
            var token = User.FindFirstValue(JwtClaimType.Token);
            var response = await _userService.GetUserProfileAsync(token);
            if(response.IsSuccess)
            {
                var user = response.ToSuccessDataResult<UserProfile>().Data;
                var userProfileViewModel = new UserViewModel()
                {
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Email = User.FindFirstValue(JwtRegisteredClaimNames.Email),
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageUrl = user.AvatarImage,
                };

                return await Task.FromResult(View(userProfileViewModel));
            }


            return await Task.FromResult(View(new UserViewModel()));
        }

        [Authorize]
        [HttpPost]
        [Route("profile")]
        public async Task<IActionResult> Profile(UserViewModel userItemViewModel)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;

            if (userItemViewModel == null)
            {
                return RedirectToAction("profile");
            }

            var token = User.FindFirstValue(JwtClaimType.Token);
            var response = await _userService.UpdateUserProfileAsync(userItemViewModel, token);
            var message = response.Message;
            if (!response.IsSuccess)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = message;
                return View(new UserViewModel());
            }

            TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
            return RedirectToAction("profile");
        }

        [Authorize]
        [HttpPost]
        [Route("avatar")]
        public async Task<IActionResult> ChangeAvatar(IFormFile file)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var token = User.FindFirstValue(JwtClaimType.Token);

            if(file == null || file.Length <= 0)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "File upload can not be empty";
                return RedirectToAction("profile");
            }

            var content = new MultipartFormDataContent
            {
                {
                    new StreamContent(file.OpenReadStream())
                    {
                        Headers =
                    {
                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                }
                    },
                    "File",
                    file.FileName
                }
            };

            var result = await _userService.UpdateUserAvatarAsync(content, token);
            var message = result.Message;
            if (!result.IsSuccess)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = message;
                return RedirectToAction("profile");
            }

            TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
            return RedirectToAction("profile");
        }

        [Authorize]
        [HttpPost]
        [Route("changePassword")]
        public async Task<IActionResult> ChangePassword(UserPasswordViewModel userPasswordViewModel)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            if (userPasswordViewModel == null)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "Password is empty";
                return RedirectToAction("profile");
            }

            if (!userPasswordViewModel.ValidationPassword())
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "Password does not match";
                return RedirectToAction("profile");
            }

            var token = User.FindFirstValue(JwtClaimType.Token);
            var response = await _userService.ChangePassword(userPasswordViewModel, token);
            var message = response.Message;
            if (!response.IsSuccess)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = message;
                return RedirectToAction("profile");
            }

            TempData[Mode.LABEL_CONFIRM_SUCCESS] = message;
            return RedirectToAction("profile");
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
