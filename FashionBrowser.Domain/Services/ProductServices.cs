using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FashionBrowser.Domain.Model;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FashionBrowser.Domain.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IUrlService _urlService;
        private readonly HttpClient _httpClient;
        public bool _isSuccess;
        public string[] _errorDetail;

        public ProductServices(IUrlService urlService, HttpClient httpClient)
        {
            _urlService = urlService;
            _httpClient = httpClient;
        }

        public async Task<List<ProductItemViewModel>> GetListProductAsync()
        {
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/products";
                var response = await _httpClient.GetAsync(apiUrl);

                var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<ProductItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());
                _isSuccess = responseList.IsSuccess;
                _errorDetail = responseList.ErrorsDetail;

                var products = responseList.Data;
                foreach (var product in products)
                {
                    product.ImageUrl = _urlService.GetFileApiUrl(product.MainImageName);
                }

                return products;
            }
            catch (Exception exception)
            {
                _errorDetail = new string[] { exception.InnerException.Message };
                return null;
            }
        }

        public async Task<Tuple<ProductItemViewModel, string>> GetProductByIdAsync(string productId)
        {
            var message = "";
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/products/";
                var response = await _httpClient.GetAsync(apiUrl + productId);
                var responseList = JsonConvert.DeserializeObject<ResponseAPI<ProductItemViewModel>>
                                   (await response.Content.ReadAsStringAsync());
                var productDto = responseList.Data;
                message = responseList.Message;

                productDto.ImageUrl = _urlService.GetFileApiUrl(productDto.MainImageName);
                return Tuple.Create(productDto, message);
            }
            catch (Exception exception)
            {
                message = exception.InnerException.Message + "Error! An error occurred. ! ";
                return Tuple.Create(default(ProductItemViewModel), message);
            }
        }
        public async Task<ProductViewModel> GetProductViewModelAsync()
        {
            var productViewModel = new ProductViewModel();
            productViewModel.ListProduct = await GetListProductAsync();

            productViewModel.IsSuccess = _isSuccess;
            productViewModel.ErrorDetail = _errorDetail;

            return productViewModel;
        }
    }
}