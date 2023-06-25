using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace FashionBrowser.Domain.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUrlService _urlService;
        private readonly HttpClient _httpClient;
        public bool _isSuccess;
        public string[] _errorDetail;

        public CategoryServices(IUrlService urlService, HttpClient httpClient)
        {
            _urlService = urlService;
            _httpClient = httpClient;
        }

        public async Task<CategoryViewModel> GetCategoryViewModelAsync()
        {
            var categoryViewModel = new CategoryViewModel();
            categoryViewModel.ListCategory = await GetListCategoryItemAsync();
            categoryViewModel.IsSuccess = _isSuccess;
            categoryViewModel.ErrorDetail = _errorDetail;

            return categoryViewModel;
        }

        public async Task<List<CategoryItemViewModel>> GetListCategoryItemAsync()
        {
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/categories";
                var response = await _httpClient.GetAsync(apiUrl);

                var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<CategoryItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());
                _isSuccess = responseList.IsSuccess;
                _errorDetail = responseList.ErrorsDetail;

                var categories = responseList.Data;
                foreach (var category in categories)
                {
                    category.ImageUrl = _urlService.GetFileApiUrl(category.ImageName);
                }

                return categories;
            }
            catch (Exception exception)
            {
                _errorDetail = new string[] { exception.InnerException.Message };
                return null;
            }
        }

        public async Task<ProductViewModel> GetProductsItemAsync(int categoryCode)
        {
            var apiUrl = _urlService.GetBaseUrl() + "/api/categories/products/";

            var response = await _httpClient.GetAsync(apiUrl + categoryCode);
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<ProductItemViewModel>>>
                                 (await response.Content.ReadAsStringAsync());

            _isSuccess = responseList.IsSuccess;
            _errorDetail = responseList.ErrorsDetail;
            var products = responseList.Data;
            var productView = new ProductViewModel();
            productView.IsSuccess = _isSuccess;
            var categories = await GetListCategoryItemAsync();

            if(products.Count > 0)
            {
                productView.CateName = categories.Where(c => c.Id == products[0].CategoryId).FirstOrDefault().Name;
                if (_isSuccess)
                {
                    foreach (var product in products)
                    {
                        product.ImageUrl = _urlService.GetFileApiUrl(product.MainImageName);
                    }
                    productView.ListProduct = products;
                }
            }    
           
            return productView;
        }
    }
}