using FashionBrowser.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly AppDbContext _appDataContext;
		public ProductRepository(AppDbContext appDataContext)
		{
			this._appDataContext = appDataContext;
		}
		public async Task<List<Product>> GetListAsync()
		{
			 return await _appDataContext.Products.ToListAsync();
		}

		public async Task<Product> GetProductByIdAsync(Guid id)
		{
			var product = await _appDataContext.Products.Where(p => p.Id == id)
														.FirstOrDefaultAsync();
			return product;
		}
	}
}
