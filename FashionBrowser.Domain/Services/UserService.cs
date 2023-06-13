
using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly TokenConfig _tokenConfig;
        public bool _isSuccess;

        public UserService(IUrlService urlService, HttpClient httpClient, IOptions<TokenConfig> options)
        {
            _urlService = urlService;
            _httpClient = httpClient;
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
    }
}
