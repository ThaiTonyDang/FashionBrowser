
using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.Model.Files;
using FashionBrowser.Domain.Model.Users;
using FashionBrowser.Domain.Services.HttpClients;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Domain.ViewModels.Users;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IFileService _fileService;
        private readonly TokenConfig _tokenConfig;
        private const string _apiResource = "users";
        private readonly IHttpClientService _httpClientService;

        public UserService(IOptions<TokenConfig> options, IHttpClientService httpClientService, IUrlService urlService, IFileService fileService)
        {
            _urlService = urlService;
            _tokenConfig = options.Value;
            _httpClientService = httpClientService;
            _fileService = fileService;
        }

        public async Task<ResultDto> RegisterUserAsync(RegisterItemViewModel registerUser)
		{
            var apiUrl = $"{_apiResource}/register";
            var response = await _httpClientService.PostAsync(registerUser, apiUrl);
            return response;
        }

        public async Task<ResultDto> GetUserProfileAsync(string token)
        {
            var apiUrl = $"{_apiResource}/profile";
            var response = await _httpClientService.GetDataAsync<UserProfile>(apiUrl, token);
            if (response.IsSuccess)
            {
                var user = response.ToSuccessDataResult<UserProfile>().Data;
                user.AvatarImage = _urlService.GetFileApiUrl(user.AvatarImage);
                return new SuccessDataResult<UserProfile>(response.Message, user);
            }

            return response;
        }

        public async Task<ResultDto> UpdateUserProfileAsync(UserViewModel userItemViewModel, string token)
        {
            var apiUrl = $"{_apiResource}/profile";
            var response = await _httpClientService.PatchAsync(userItemViewModel, apiUrl, token);
            return response;
        }

        public async Task<ResultDto> UpdateUserAvatarAsync(MultipartFormDataContent file, string token)
        {
            var fileResponse = await _fileService.UploadFileAsync(file, token);
            if (fileResponse.IsSuccess)
            {
                var fileUpload = fileResponse.ToSuccessDataResult<FileUpload>().Data;
                var avatar = fileUpload.FileName;
                var apiUrl = $"{_apiResource}/profile/avatar";
                var response = await _httpClientService.PatchAsync(avatar, apiUrl, token);
                return response;
            }

            return fileResponse;
        }

        public async Task<ResultDto> ChangePassword(UserPasswordViewModel userPasswordViewModel, string token)
        {
            var apiUrl = $"{_apiResource}/profile/change-password";
            var response = await _httpClientService.PatchAsync(userPasswordViewModel, apiUrl, token);
            return response;
        }

        public async Task<ResultDto> LoginAsync(UserLoginViewModel loginUser)
        {
            var apiUrl = $"{_apiResource}/login";
            var response = await _httpClientService.PostDataAsync<UserLoginViewModel, string>(loginUser, apiUrl);
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
            claims.Add(new Claim(JwtClaimType.Token, token));
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
