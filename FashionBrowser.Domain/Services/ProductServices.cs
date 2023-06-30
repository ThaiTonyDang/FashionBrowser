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
using FashionBrowser.Domain.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using FashionBrowser.Domain.Dto;

namespace FashionBrowser.Domain.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IUrlServices _urlService;
        private readonly HttpClient _httpClient;
        private readonly PageConfig _pageConfig;
        public bool _isSuccess;
        public string _message;
        public string[] _errorDetail;

        public ProductServices(IUrlServices urlService, HttpClient httpClient, IOptions<PageConfig> pageOptions)
        {
            _urlService = urlService;
            _httpClient = httpClient;
            _pageConfig = pageOptions.Value;
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

        public async Task<List<ProductItemViewModel>> GetPagingProductListAsync(int currentPage)
        {
            var apiUrl = _urlService.GetBaseUrl() + "/api/products";
            var pageSize = _pageConfig.PageSize;
            var response = await _httpClient.GetAsync(apiUrl + $"?currentpage={currentPage}&pagesize={pageSize}");
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<ProductItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());

            _isSuccess = responseList.IsSuccess;
            _message = responseList.Message;
            var products = responseList.Data;
            if (products != null)
            {
                foreach (var product in products)
                {
                    product.ImageUrl = _urlService.GetFileApiUrl(product.MainImageName);
                }
            }

            return products;
        }

        public async Task<ProductViewModel> GetPagingProductViewModel(int currentPage)
        {
            var productViewModel = new ProductViewModel();
            var pageSize = _pageConfig.PageSize;
            productViewModel.ListProduct = await GetPagingProductListAsync(currentPage);
            productViewModel.IsSuccess = _isSuccess;
            var totalItems = await GetTotalItems();
            productViewModel.Paging = new Paging(currentPage, pageSize, totalItems);

            return productViewModel;
        }

        private async Task<int> GetTotalItems()
        {
            var apiUrl = _urlService.GetBaseUrl() + "/api/products/";
            var response = await _httpClient.GetAsync(apiUrl + "total-products");
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<int>>
                               (await response.Content.ReadAsStringAsync());
            var totalItems = responseList.Data;

            return totalItems;
        }
    }
}