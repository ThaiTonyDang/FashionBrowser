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
    public class OrderServices : IOrderServices
    {
        private readonly IUrlServices _urlService;
        private readonly HttpClient _httpClient;
        public OrderServices(IUrlServices urlService, HttpClient httpClient)
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

        public async Task<bool> UpdatePaidStatus(OrderDto order, string token)
        {
            var apiUrl = _urlService.GetBaseUrl() + "/api/orders/paid-status-update";
            _httpClient.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var response = await _httpClient.PutAsJsonAsync(apiUrl , order);
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<bool>>
                                    (await response.Content.ReadAsStringAsync());
            var isSuccess = responseList.IsSuccess;
            return isSuccess;
        }
    }
}
