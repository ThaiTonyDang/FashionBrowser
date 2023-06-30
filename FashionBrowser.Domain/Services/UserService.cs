
using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.Services.HttpClients;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace FashionBrowser.Domain.Services
{
    public class UserService : IUserService
	{
        private readonly IFileService _fileService;
        private readonly TokenConfig _tokenConfig;
        private const string _apiPathUrl = "users";
        private readonly IHttpClientService _httpClientService;

        public UserService(IFileService fileService, IOptions<TokenConfig> options, IHttpClientService httpClientService)
        {
            _fileService = fileService;
            _tokenConfig = options.Value;
            _httpClientService = httpClientService;
        }

        public async Task<ResultDto> RegisterUserAsync(RegisterItemViewModel registerUser)
		{
            var apiUrl = $"{_apiPathUrl}/register";
            var response = await _httpClientService.PostAsync(registerUser, apiUrl);
            return response;
        }

        public async Task<Tuple<bool, string>> UpdateUserAsync(UserItemViewModel userItemViewModel, string token)
        {
            
            //var urlApi = _urlService.GetBaseUrl() + "/api/users/update";
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            //                    JwtBearerDefaults.AuthenticationScheme, token);
            //var response = await _httpClient.PutAsJsonAsync(urlApi, userItemViewModel);
            
            //var isSuccess = response.IsSuccessStatusCode;
            //if(isSuccess)
            //{
            //    return Tuple.Create(isSuccess, "Update Information Success !");
            //}
            return Tuple.Create(false, "Update Information Fail !");
        }

        public async Task<Tuple<bool, string>> UpdateUserAvatarAsync(UserItemViewModel userItemViewModel, string token)
        {
            //var file = userItemViewModel.File;
            //var fileName = "";
            //if (file != null)
            //{
            //    var dataList = await _fileService.GetResponeUploadFileAsync(file, _httpClient, token);
            //    if (dataList != null)
            //    {
            //        fileName = dataList[0];
            //    }
            //}
            //if (!string.IsNullOrEmpty(userItemViewModel.AvatarImage) && file == null)
            //{
            //    fileName = userItemViewModel.AvatarImage;
            //}
            //userItemViewModel.AvatarImage = fileName;
            //var urlApi = _urlService.GetBaseUrl() + "/api/users/avatar-update";
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            //            JwtBearerDefaults.AuthenticationScheme, token);
            //var response = await _httpClient.PutAsJsonAsync(urlApi, userItemViewModel);
            //var isSuccess = response.IsSuccessStatusCode;
            //if (isSuccess)
            //{
            //    return Tuple.Create(isSuccess, "Change Avatar Success !");
            //}
            return Tuple.Create(false, "Change Avatar Fail !");
        }

        public async Task<UserItemViewModel> GetUserProfileAsync(string token)
        {
            var apiUrl = $"{_apiPathUrl}/profile";
            var response = await _httpClientService.PostAsync(registerUser, apiUrl);

            var urlApi = _urlService.GetBaseUrl() + "/api/users/single-user";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                       JwtBearerDefaults.AuthenticationScheme, token);
            var response = await _httpClient.GetAsync(urlApi);
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<UserItemViewModel>>
                                  (await response.Content.ReadAsStringAsync());
            if (response.IsSuccessStatusCode)
            {
                var user = responseList.Data;
                user.ImageUrl = _urlService.GetFileApiUrl(user.AvatarImage);
                return user;
            }

            return null;
        }

        public async Task<Tuple<bool, string>> ChangePassword(PasswordItemViewModel passwordItemViewModel, string token)
        {
            //var urlApi = _urlService.GetBaseUrl() + "/api/users/reset-password";
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            //          JwtBearerDefaults.AuthenticationScheme, token);
            //var response = await _httpClient.PostAsJsonAsync(urlApi, passwordItemViewModel);
            //var responseList = JsonConvert.DeserializeObject<ResponseAPI<string>>
            //                     (await response.Content.ReadAsStringAsync());
            //var isSuccess = responseList.IsSuccess;
            //var statusCode = responseList.StatusCode;
            //var message = responseList.Message;
            return Tuple.Create(false, "Change Avatar Fail !");
        }

        public async Task<ResultDto> LoginAsync(LoginItemViewModel loginUser)
        {
            var apiUrl = $"{_apiPathUrl}/login";
            var response = await _httpClientService.PostDataAsync<LoginItemViewModel, string>(loginUser, apiUrl);
            if (response.IsSuccess)
            {
                var result = response.ToSuccessDataResult<string>();
                var token = result.Data;
                var isVerify = VerifyToken(token);
                if (!isVerify)
                {
                    return new ErrorResult("Token is invalid");
                }
            }

            return response;
        }

        public IEnumerable<Claim> GetClaims(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claims = jwt.Claims.ToList();
            claims.Add(new Claim(JwtClaimType.TOKEN, token));
            return claims;
        }

        private bool VerifyToken(string token)
        {
            try
            {
                var tokenParameters = GetTokenParameters();
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, tokenParameters, out securityToken);

                var isValid = principal != null && principal.Identity != null && principal.Identity.IsAuthenticated;
                return isValid;
            }
            catch (Exception)
            {
                // Log here
                
            }

            return false;
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
    }
}
