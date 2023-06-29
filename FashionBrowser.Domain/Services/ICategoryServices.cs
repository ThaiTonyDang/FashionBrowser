using FashionBrowser.Domain.ViewModels;

namespace FashionBrowser.Domain.Services
{
    public interface ICategoryServices
    {
        public Task<CategoryViewModel> GetCategoryViewModelAsync();
        public Task<List<CategoryItemViewModel>> GetCategoryListItemAsync();
        public Task<CategoryItemViewModel> GetCategoryByName(string categoryName);
        public Task<CategoryItemViewModel> GetCategoryChildrenBySlug(string childSlug);
        public Task<ProductViewModel> GetPagingProductViewByNameAsync(string categoryName, int currentPage);
        public Task<ProductViewModel> GetPagingProductViewBySlugAsync(string childSlug, int currentPage);
    }
}