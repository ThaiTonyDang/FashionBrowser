
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
	public class UserService : IUserService
	{
        private readonly IUrlService _urlService;
        private readonly HttpClient _httpClient;
        public bool _isSuccess;

        public UserService(IUrlService urlService, HttpClient httpClient)
        {
            _urlService = urlService;
            _httpClient = httpClient;
        }
        public async Task<Tuple<bool, string>> RegisterUserAsync(UserItemViewModel registerUser)
		{
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "api/users/register";
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
	}
}
