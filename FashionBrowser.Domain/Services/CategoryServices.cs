
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChelseaWeb.Domains.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryViewModel> GetCategoryViewModelAsync()
        {
            var categoryViewModel = new CategoryViewModel();
            categoryViewModel.ListCategory = await GetListCategoryItemAsync();
            return categoryViewModel;
        }

        public async Task<List<CategoryItemViewModel>> GetListCategoryItemAsync()
        {
            var categories = await _categoryRepository.GetListAsync();
            var categorieItems = categories.Select(category => new CategoryItemViewModel()
            {
                Name = category.Name,
                Description = category.Description,
                CategoryId = category.Id,
                ImagePath = category.ImagePath,
            }).ToList();

            return categorieItems;
        }
    }
}