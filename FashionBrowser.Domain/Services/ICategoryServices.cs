using FashionBrowser.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChelseaWeb.Domains.Services
{
    public interface ICategoryServices
    {
        public Task<CategoryViewModel> GetCategoryViewModelAsync();
        public Task<List<CategoryItemViewModel>> GetListCategoryItemAsync();
    }
}