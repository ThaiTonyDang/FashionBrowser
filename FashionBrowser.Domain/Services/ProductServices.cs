using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Infrastructure.Repositories;
using FashionBrowser.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChelseaWeb.Domains.Services
{
	public class ProductServices : IProductServices
	{
		private readonly IProductRepository _productRepository;

		public ProductServices(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<List<ProductItemViewModel>> GetListProductAsync()
		{
			var listProduct = await _productRepository.GetListAsync();

			var listProductViewModel = listProduct.Select(product => new ProductItemViewModel()
			{
				Id = product.Id,
				Name = product.Name,
				Provider = product.Provider,
				DisplayPrice = product.Price.GetPriceFormat(),
				Description = product.Description,
				CategoryId = product.CategoryId,
				ImagePath = product.ImagePath,
				UnitsInStock = product.UnitsInStock,
				Enable = product.Enable,
				Type = product.Type,
			}).ToList();

			return listProductViewModel;
		}

		public async Task<ProductViewModel> GetProductViewModelAsync()
		{
			var productViewModel = new ProductViewModel();
			productViewModel.ListProduct = await GetListProductAsync();
			return productViewModel;
		}	
	}
}