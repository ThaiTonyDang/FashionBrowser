using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Infrastructure.Models;
using FashionBrowser.Utilities;
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
        public void Should_DeleteCartAsync_Return_Succes()
        {
            // Arrange
            var cart = LoadCartSampletData();
            var productId = LoadCartSampletData()[1].Product.Id;
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

        [Fact]
        public void Should_AdjustCartSUBQuantityAsync_Return_Succes()
        {
            // Arrange
            var cartItem = LoadCartSampletData()[0];
            var operate = OPERATOR.SUBTRACT;
            var quantityExpect = 2;

            // Act
            var cartService = new CartServices();
            cartService.AdjustQuantity(cartItem, operate);

            // Assert
            Assert.Equal(quantityExpect, cartItem.Quantity);
        }

        [Fact]
        public void Should_AdjustCartADDQuantityAsync_Return_Succes()
        {
            // Arrange
            var cartItem = LoadCartSampletData()[0];
            var operate = OPERATOR.ADDITION;
            var quantityExpect = 4;

            // Act
            var cartService = new CartServices();
            cartService.AdjustQuantity(cartItem, operate);

            // Assert
            Assert.Equal(quantityExpect, cartItem.Quantity);
        }

        private List<CartItemViewModel> LoadCartSampletData()
        {
            return new List<CartItemViewModel>()
            {
                new CartItemViewModel()
                {
                    Quantity = 3,
                    Product = new ProductItemViewModel
                    {
                        Id = Guid.Parse("561232ff-efc8-4580-b681-4ec31beb79b8"),
                        Name = "Đồng Hồ A",
                    }
                },

                 new CartItemViewModel()
                {
                    Quantity = 2,
                    Product = new ProductItemViewModel
                    {
                        Id = Guid.Parse("2c4b115e-ad9a-4259-8732-1f23e019802b"),
                        Name = "Đồng Hồ B",
                    }
                },

                 new CartItemViewModel()
                {
                    Quantity = 4,
                    Product = new ProductItemViewModel
                    {
                        Id = Guid.Parse("f4a55c69-46db-499b-998e-2bdeb01166e0"),
                        Name = "Đồng Hồ C",
                    }
                },
            };
        }
    }
}