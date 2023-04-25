using FashionBrowser.Infrastructure.DataContext;
using FashionBrowser.Infrastructure.Models;
using FashionBrowser.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FashionBrowser.Test.RepositoriesTest
{
	public class ProductRepositoryTest
	{
		private readonly AppDbContext _dbContext;
		private readonly ProductRepository _productRepository;
		public ProductRepositoryTest()
		{
			var appContextOptions = new DbContextOptionsBuilder<AppDbContext>()
								   .UseInMemoryDatabase(databaseName: $"Fashion_{DateTime.Now.ToFileTimeUtc()}")
								   .Options;

			_dbContext = new AppDbContext(appContextOptions);
			_productRepository = new ProductRepository(_dbContext);

			SeddingDataAsync(_dbContext)
							.ConfigureAwait(true)
							.GetAwaiter()
							.GetResult();
		}

		private List<Product> LoadProducSampletData()
		{
			var listCates = LoadCategorySampleData();

			return new List<Product>()
			{
				new Product()
				{
					Id = Guid.Parse("561232ff-efc8-4580-b681-4ec31beb79b8"),
					Name = "Đồng Hồ",
					Provider = "Chelsea Fc",
					Price = 10,
					CategoryId = listCates[0].Id,
					Description = "Đồng Hồ Chelsea Chính Hãng",
					ImagePath = "Pepsi20230216034120.jpg",
					Enable = false
				},

				new Product()
				{
					Id = Guid.Parse("2c4b115e-ad9a-4259-8732-1f23e019802b"),
					Name = "Áo Choàng Mùa Đông Chelsea",
					Provider = "Fashion VN",
					Price = 100,
					CategoryId = listCates[1].Id,
					Description = "Áo Choàng Thời Trang",
					ImagePath = "pho-bo20230216053008.jpg",
					Enable = true
				},

				new Product()
				{
					Id = Guid.Parse("f4a55c69-46db-499b-998e-2bdeb01166e0"),
					Name = "Mũ Lưỡi Trai",
					Provider = "Valve",
					Price = 10,
					CategoryId = listCates[2].Id,
					Description = "Mũ",
					ImagePath = "pho-bo20230216053008.jpg",
					Enable = true
				}
			};
		}

		private async Task SeddingDataAsync(AppDbContext appDbContext)
		{
			var categories = LoadCategorySampleData();
			var products = LoadProducSampletData();

			await appDbContext.Categories.AddRangeAsync(categories);
			await appDbContext.Products.AddRangeAsync(products);

			await appDbContext.SaveChangesAsync();
		}

		private List<Category> LoadCategorySampleData()
		{   
			return new List<Category>()
			{
				new Category
				{
					Id = Guid.NewGuid(),
					Name = "Trang Sức",
					Description = "Trang Sức"
				},

				new Category
				{
					Id = Guid.NewGuid(),
					Name = "Áo Choàng",
					Description = "Food Type"
				},

				new Category
				{
					Id = Guid.NewGuid(),
					Name = "Mũ",
					Description = "Entertainment Type"
				}
			};
		}
	}
}