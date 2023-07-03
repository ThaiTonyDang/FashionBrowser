using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace FashionBrowser.Domain.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUrlServices _urlService;
        private readonly HttpClient _httpClient;
        private readonly PageConfig _pageConfig;
        public bool _isSuccess;
        public string[] _errorDetail;

        public CategoryServices(IUrlServices urlService, HttpClient httpClient, IOptions<PageConfig> options)
        {
            _urlService = urlService;
            _httpClient = httpClient;
            _pageConfig = options.Value;
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
 
        public async Task<CategoryItemViewModel> GetCategoryByName(string categoryName)
        {
            var categoryItemViews = await GetCategoryListItemAsync();
            var category = categoryItemViews.Where(c => c.Name.Equals(categoryName)).FirstOrDefault();
            return category;
        }

        public async Task<CategoryItemViewModel> GetCategoryChildrenBySlug(string childSlug)
        {
            var categoryItemViews = await GetCategoryListItemAsync();
            foreach (var category in categoryItemViews)
            {
                foreach (var child in category.CategoryChildrens)
                {
                    if (child.Slug.Equals(childSlug))
                    {
                        return category;
                    }
                }
            }

            return null;
        }

        public async Task<ProductViewModel> GetPagingProductViewByNameAsync(string categoryName, int currentPage)
        {
            var productViewModel = new ProductViewModel();
            var pagingProducts = await GetAllProductsByCategoryNameAsync(categoryName, currentPage);
            var pageSize = _pageConfig.PageSize;
            var totalItems = (await GetProductsByCategoryName(categoryName)).Count;
            productViewModel.ListProductCategory = pagingProducts;
            productViewModel.Paging = new Paging(currentPage, pageSize, totalItems);
            return productViewModel;
        }

        public async Task<ProductViewModel> GetPagingProductViewBySlugAsync(string childSlug, int currentPage)
        {
            var productViewModel = new ProductViewModel();
            var pagingProducts = await GetAllProductsBySlugAsync(childSlug, currentPage);
            var pageSize = _pageConfig.PageSize;
            var totalItems = (await GetProductsChildrenBySlug(childSlug)).Count;
            productViewModel.ListProductCategory = pagingProducts;
            productViewModel.Paging = new Paging(currentPage, pageSize, totalItems);
            return productViewModel;
        }

        private async Task<List<ProductItemViewModel>> GetAllProductsByCategoryNameAsync(string categoryName, int currentPage)
        {
            var pageSize = _pageConfig.PageSize;
            var products = await GetProductsByCategoryName(categoryName);

            var pagingProducts = products.OrderBy(p => p.Name)
                                         .Skip((currentPage - 1) * pageSize)
                                         .Take(pageSize).AsQueryable()
                                         .ToList();
            return await Task.FromResult(pagingProducts);
        }

        private async Task<List<ProductItemViewModel>> GetAllProductsBySlugAsync(string childSlug, int currentPage)
        {
            var pageSize = _pageConfig.PageSize;
            var products = await GetProductsChildrenBySlug(childSlug);

            var pagingProducts = products.OrderBy(p => p.Name)
                                         .Skip((currentPage - 1) * pageSize)
                                         .Take(pageSize).AsQueryable()
                                         .ToList();
            return await Task.FromResult(pagingProducts);
        }

        private async Task<List<ProductItemViewModel>> GetProductsChildrenBySlug(string childSlug)
        {
            var category = await GetCategoryChildrenBySlug(childSlug);
            var child = category.CategoryChildrens.Where(c => c.Slug.Equals(childSlug)).FirstOrDefault();
            var products = child.ProductDtos.ToList();
            return products;
        }

        private async Task<List<ProductItemViewModel>> GetProductsByCategoryName(string categoryName)
        {
            var category = await GetCategoryByName(categoryName);
            if (category == null)
            {
                return default;
            }

            var products = new List<ProductItemViewModel>();
            if (category != null || category.CategoryChildrens != null)
            {
                foreach (var item in category.CategoryChildrens)
                {
                    products.AddRange(item.ProductDtos);
                }
            }

            return products;
        }
    }
}