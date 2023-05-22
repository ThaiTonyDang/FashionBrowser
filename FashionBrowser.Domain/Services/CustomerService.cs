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
        public async Task<Tuple<bool, string>> CreateCustomerInfor(CustomerItemViewModel customerView)
        {
            var message = "";
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "api/customers";
                var response = await _httpClient.PostAsJsonAsync(apiUrl, customerView);
                var responseList = JsonConvert.DeserializeObject<ResponseAPI<ProductItemViewModel>>
                                    (await response.Content.ReadAsStringAsync());
                message = responseList.Message;
                return Tuple.Create(true, message + " ! ");
            }
            catch (Exception exception)
            {
                message = exception.InnerException.Message + " ! " + "Create Customer Fail !";
                return Tuple.Create(false, message);
            }
        }
    }
}
