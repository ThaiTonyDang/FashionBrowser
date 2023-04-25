using FashionBrowser.Infrastructure.DataContext;
using FashionBrowser.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDataContext)
        {
            _appDbContext = appDataContext;
        }

        public async Task<List<Category>> GetListAsync()
        {
            var categories = await _appDbContext.Categories.ToListAsync();
            return categories;
        }
    }
}