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
    public class CustomerService : ICustomerService
    {
        private readonly IUrlService _urlService;
        private readonly HttpClient _httpClient;
        public CustomerService(IUrlService urlService, HttpClient httpClient)
        {
            _urlService = urlService;
            _httpClient = httpClient;
        }
        public async Task<bool> CreateCustomerInfor(CustomerItemViewModel customerView)
        {
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "api/customers";
                var response = await _httpClient.PostAsJsonAsync(apiUrl, customerView);
                var responseList = JsonConvert.DeserializeObject<ResponseAPI<ProductItemViewModel>>
                                    (await response.Content.ReadAsStringAsync());
                
                return responseList.Success; ;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}
