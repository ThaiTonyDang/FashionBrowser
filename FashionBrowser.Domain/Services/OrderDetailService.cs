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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUrlService _urlService;
        private readonly HttpClient _httpClient;
        public OrderDetailService(IUrlService urlService, HttpClient httpClient)
        {
            _urlService = urlService;
            _httpClient = httpClient;
        }
        public async Task<bool> CreateOrderDetail(OrderDetailItemViewModel orderDetailItem)
        {
            var message = "";         
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/orderdetails";
                var response = await _httpClient.PostAsJsonAsync(apiUrl, orderDetailItem);
                var responseList = JsonConvert.DeserializeObject<ResponseAPI<ProductItemViewModel>>
                                    (await response.Content.ReadAsStringAsync());
              
                return responseList.IsSuccess;
            }
            catch (Exception exception)
            {
                message = exception.InnerException.Message + " ! " + "Create Order Fail !";
                return false;
            }           
        }
    }
}
