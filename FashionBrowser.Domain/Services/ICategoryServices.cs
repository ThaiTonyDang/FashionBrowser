using FashionBrowser.Domain.ViewModels;

namespace FashionBrowser.Domain.Services
{
    public interface ICategoryServices
    {
        public Task<CategoryViewModel> GetCategoryViewModelAsync();
        public Task<List<CategoryItemViewModel>> GetCategoryListItemAsync();
        public Task<CategoryItemViewModel> GetCategoryByName(List<CategoryItemViewModel> categoryItemViews, string categoryName);
        public Tuple<List<ProductItemViewModel>, CategoryItemViewModel> GetProductCategoryChildren(List<CategoryItemViewModel> categoryItemViews, string childSlug);
        public Task<List<ProductItemViewModel>> GetAllProductsByCategoryName(List<CategoryItemViewModel> categories, string categoryName);
    }
}