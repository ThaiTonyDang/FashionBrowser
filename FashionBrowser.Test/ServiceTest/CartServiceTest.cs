using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FashionBrowser.Test.ServiceTest
{
    public class CartServiceTest
    {
        [Fact]
        public void  Should_DeleteCartAsync_Return_Succes()
        {
            // Arrange
            var cart = LoadCartSampletData();
            var productId = LoadCartSampletData()[1].ProductItemViewModel.Id;
            // Act
            var cartService = new CartServices();
            var isSuccess = cartService.DeleteCartItems(cart, productId);

            // Assert
            Assert.True(isSuccess);
        }

        [Fact]
        public void Should_DeleteCartAsync_Return_Fail_When_CartItem_Null()
        {
            // Arrange
            var cart = LoadCartSampletData();
            var productId = Guid.NewGuid();
            // Act
            var cartService = new CartServices();
            var isSuccess = cartService.DeleteCartItems(cart, productId);

            // Assert
            Assert.False(isSuccess);
        }

        private List<CartItemViewModel> LoadCartSampletData()
        {
            return new List<CartItemViewModel>()
            {
                new CartItemViewModel()
                {
                    Quantity = 3,
                    ProductItemViewModel = new ProductItemViewModel
                    {
                        Id = Guid.Parse("561232ff-efc8-4580-b681-4ec31beb79b8"),
                        Name = "Đồng Hồ A",
                    }                 
                },

                 new CartItemViewModel()
                {
                    Quantity = 2,
                    ProductItemViewModel = new ProductItemViewModel
                    {
                        Id = Guid.Parse("2c4b115e-ad9a-4259-8732-1f23e019802b"),
                        Name = "Đồng Hồ B",
                    }
                },

                 new CartItemViewModel()
                {
                    Quantity = 4,
                    ProductItemViewModel = new ProductItemViewModel
                    {
                        Id = Guid.Parse("f4a55c69-46db-499b-998e-2bdeb01166e0"),
                        Name = "Đồng Hồ C",
                    }
                },
            };
        }

    }
}
