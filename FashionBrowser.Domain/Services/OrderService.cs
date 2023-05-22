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
    public class OrderService : IOrderService
    {
        private readonly IUrlService _urlService;
        private readonly HttpClient _httpClient;
        public OrderService(IUrlService urlService, HttpClient httpClient)
        {
            _urlService = urlService;
            _httpClient = httpClient;
        }
        public async Task<Tuple<bool, string>> CreateOrder(OrderItemViewModel orderItemViewModel)
        {
            var message = "";
            if (orderItemViewModel != null)
            {              
                try
                {
                    var apiUrl = _urlService.GetBaseUrl() + "api/orders";
                    var response = await _httpClient.PostAsJsonAsync(apiUrl, orderItemViewModel);
                    var responseList = JsonConvert.DeserializeObject<ResponseAPI<ProductItemViewModel>>
                                       (await response.Content.ReadAsStringAsync());
                    message = responseList.Message;

                    if (responseList.Success)
                    {
                        return Tuple.Create(true, message + " ! ");
                    }
                }
                catch (Exception exception)
                {
                    message = exception.InnerException.Message + " ! " + "Create Order Fail !";
                    return Tuple.Create(false, message);
                }
            }

            return Tuple.Create(false, message);
        }
    }
}
