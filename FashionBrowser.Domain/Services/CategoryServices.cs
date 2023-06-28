using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
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
            categoryViewModel.ListCategory = await GetCategoryListItemAsync();
            categoryViewModel.IsSuccess = _isSuccess;
            categoryViewModel.ErrorDetail = _errorDetail;

            return categoryViewModel;
        }

        public async Task<List<CategoryItemViewModel>> GetCategoryListItemAsync()
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
                    foreach (var child in category.CategoryChildrens)
                    {
                        child.ImageUrl = _urlService.GetFileApiUrl(child.ImageName);
                        foreach (var product in child.ProductDtos)
                        {
                            product.ImageUrl = _urlService.GetFileApiUrl(product.MainImageName);
                        }
                    }
                }

                return categories;
            }
            catch (Exception exception)
            {
                _errorDetail = new string[] { exception.InnerException.Message };
                return null;
            }
        }
 
        public Task<CategoryItemViewModel> GetCategoryByName(List<CategoryItemViewModel> categoryItemViews, string categoryName)
        {
            var category = categoryItemViews.Where(c => c.Name.Equals(categoryName)).FirstOrDefault();
            return Task.FromResult(category);
        }

        public Tuple<List<ProductItemViewModel>, CategoryItemViewModel> GetProductCategoryChildren(List<CategoryItemViewModel> categoryItemViews, string childSlug)
        {
            foreach (var category in categoryItemViews)
            {
                foreach (var child in category.CategoryChildrens)
                {
                    if (child.Slug.Equals(childSlug))
                    {
                        return Tuple.Create(child.ProductDtos.ToList(), category);
                    }
                }
            }

            return Tuple.Create(new List<ProductItemViewModel>(), default(CategoryItemViewModel));

        }

        public async Task<List<ProductItemViewModel>> GetAllProductsByCategoryName(List<CategoryItemViewModel> categories, string categoryName)
        {
            var category = categories.Where(c => c.Name.ToLower().Equals(categoryName.ToLower())).FirstOrDefault();
            if (category == null)
            {
                return await Task.Run(() => default(List<ProductItemViewModel>));
            }

            var products = new List<ProductItemViewModel>();
            if (category != null || category.CategoryChildrens != null)
            {
                foreach (var item in category.CategoryChildrens)
                {
                    products.AddRange(item.ProductDtos);
                }
            }

            return await Task.Run(() => products);
        }
    }
}