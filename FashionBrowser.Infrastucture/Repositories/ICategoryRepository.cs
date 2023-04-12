using FashionBrowser.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Infrastructure.Repositories
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetListAsync();
    }
}
