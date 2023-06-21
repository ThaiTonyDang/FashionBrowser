
using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace FashionBrowser.Domain.Services
{
    public class UserService : IUserService
	{
        private readonly IUrlService _urlService;
        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;
        private readonly TokenConfig _tokenConfig;
        public bool _isSuccess;

        public UserService(IUrlService urlService, IFileService fileService,  HttpClient httpClient, IOptions<TokenConfig> options)
        {
            _urlService = urlService;
            _httpClient = httpClient;
            _fileService = fileService;
            _tokenConfig = options.Value;
        }

        public async Task<Tuple<ClaimsPrincipal, bool, string>> LoginAsync(LoginItemViewModel loginUser)
        {
            var message = "";
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/users/login";
                var response = await _httpClient.PostAsJsonAsync(apiUrl, loginUser);
                var data = JsonConvert.DeserializeObject<ResponseAPI<string>>(await response.Content.ReadAsStringAsync());
                message = data.Message;
                if (data.IsSuccess)
                {
                    var token = data.Data;
                    var result = VerifyToken(token);
                    var claimsPrincipal = result.Item1;
                    var isVerify = result.Item2;
                    message = result.Item3;                  

                    return Tuple.Create(claimsPrincipal, true, token);
  
                }    
            }
            catch (Exception e)
            {
                return Tuple.Create(default(ClaimsPrincipal), false, e.Message);
            }

            return Tuple.Create(default(ClaimsPrincipal), false, message);
        }

        public async Task<Tuple<bool, string>> RegisterUserAsync(UserItemViewModel registerUser)
		{
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/users/register";
                var response = await _httpClient.PostAsJsonAsync(apiUrl, registerUser);

                var responseList = JsonConvert.DeserializeObject<ResponseAPI<UserItemViewModel>>
                                   (await response.Content.ReadAsStringAsync());
                _isSuccess = responseList.IsSuccess;

                return Tuple.Create(_isSuccess,  responseList.Message);
            }
            catch (Exception exception)
            {               
                return Tuple.Create(false, exception.Message);
            }
        }

        public IEnumerable<Claim> GetClaims(ClaimsPrincipal claimsPrincipal, string token)
        {
            var claims = claimsPrincipal.Claims.ToList();
            claims.Add(new Claim("token", token));
            return claims;
        }

        private Tuple<ClaimsPrincipal, bool, string> VerifyToken(string token)
        {
            try
            {
                var tokenParameters = GetTokenParameters();
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, tokenParameters, out securityToken);

                if (principal != null && principal.Identity != null && principal.Identity.IsAuthenticated)
                    return Tuple.Create(principal, true, "Verify Success !");
                return Tuple.Create(default(ClaimsPrincipal), false, "Verify Fail !");
            }
            catch (Exception e)
            {
                var error = e.Message;
                return Tuple.Create(default(ClaimsPrincipal), false, error);
            }
        }

        private TokenValidationParameters GetTokenParameters()
        {
            var tokenOptions = new TokenValidationParameters()
            {
                ValidIssuer = _tokenConfig.Issuer,
                ValidAudience = _tokenConfig.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret))
            };

            return tokenOptions;
        }

        public async Task<Tuple<bool, string>> UpdateUserAsync(UserItemViewModel userItemViewModel, string token)
        {
            
            var urlApi = _urlService.GetBaseUrl() + "/api/users/update";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                JwtBearerDefaults.AuthenticationScheme, token);
            var response = await _httpClient.PutAsJsonAsync(urlApi, userItemViewModel);
            
            var isSuccess = response.IsSuccessStatusCode;
            if(isSuccess)
            {
                return Tuple.Create(isSuccess, "Update Information Success !");
            }
            return Tuple.Create(false, "Update Information Fail !");
        }

        public async Task<Tuple<bool, string>> UpdateUserAvatarAsync(UserItemViewModel userItemViewModel, string token)
        {
            var file = userItemViewModel.File;
            var fileName = "";
            if (file != null)
            {
                var dataList = await _fileService.GetResponeUploadFileAsync(file, _httpClient, token);
                if (dataList != null)
                {
                    fileName = dataList[0];
                }
            }
            if (!string.IsNullOrEmpty(userItemViewModel.AvatarImage) && file == null)
            {
                fileName = userItemViewModel.AvatarImage;
            }
            userItemViewModel.AvatarImage = fileName;
            var urlApi = _urlService.GetBaseUrl() + "/api/users/avatar-update";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        JwtBearerDefaults.AuthenticationScheme, token);
            var response = await _httpClient.PutAsJsonAsync(urlApi, userItemViewModel);
            var isSuccess = response.IsSuccessStatusCode;
            if (isSuccess)
            {
                return Tuple.Create(isSuccess, "Change Avatar Success !");
            }
            return Tuple.Create(false, "Change Avatar Fail !");
        }

        public async Task<UserItemViewModel> GetUserAsync(string token)
        {
            var urlApi = _urlService.GetBaseUrl() + "/api/users/user";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                       JwtBearerDefaults.AuthenticationScheme, token);
            var response = await _httpClient.GetAsync(urlApi);
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<UserItemViewModel>>
                                  (await response.Content.ReadAsStringAsync());
            if (response.StatusCode == (HttpStatusCode)404) return new UserItemViewModel();
            var user = responseList.Data;
            user.ImageUrl = _urlService.GetFileApiUrl(user.AvatarImage);
            return user;
        }

        public async Task<Tuple<bool, string>> ChangePassword(PasswordItemViewModel passwordItemViewModel, string token)
        {
            var urlApi = _urlService.GetBaseUrl() + "/api/users/reset-password";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                      JwtBearerDefaults.AuthenticationScheme, token);
            var response = await _httpClient.PostAsJsonAsync(urlApi, passwordItemViewModel);
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<string>>
                                 (await response.Content.ReadAsStringAsync());
            var isSuccess = responseList.IsSuccess;
            var statusCode = responseList.StatusCode;
            var message = responseList.Message;
            return Tuple.Create(isSuccess, message);
        }
    }
}
