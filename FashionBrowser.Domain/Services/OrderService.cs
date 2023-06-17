using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<bool> CreateOrder(OrderDto orderDto, string token)
        {
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/orders/ordercreate";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

                var response = await _httpClient.PostAsJsonAsync(apiUrl, orderDto);

                var responseList = JsonConvert.DeserializeObject<ResponseAPI<ProductItemViewModel>>
                                    (await response.Content.ReadAsStringAsync());

                return responseList.IsSuccess;

            }
            catch
            {
                return false;
            }
        }
    }
}
