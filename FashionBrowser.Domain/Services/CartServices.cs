using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
	public class CartServices : ICartServices
	{
		public void AddToCart(ProductItemViewModel product, List<CartItemViewModel> carts, int quantityInput)
		{
			var cartitem = carts.Find(item => item.ProductItemViewModel.Id == product.Id);
			if (cartitem != null)
			{
				if (quantityInput == 0)
				{
					cartitem.Quantity++;
				}
				else
				{
					cartitem.Quantity += quantityInput;
				}
			}
			else
			{
				carts.Add(new CartItemViewModel() { Quantity = 1, ProductItemViewModel = product });
			}
		}

		public bool DeleteCartItems(List<CartItemViewModel> carts, Guid id)
		{
			var cartItem = GetCartItemByProductId(carts, id);
			if (cartItem != null)
			{
				carts.Remove(cartItem);
				return true;
			}

			return false;
		}

		public CartViewModel GetCartViewModel(List<CartItemViewModel> carts)
		{
			var cartViewModel = new CartViewModel();
			cartViewModel.ListCartItem = carts;
			return cartViewModel;
		}

		public CartItemViewModel GetCartItemByProductId(List<CartItemViewModel> carts, Guid id)
		{
			var cartitem = carts.Find(item => item.ProductItemViewModel.Id == id);
			return cartitem;
		}
	}
}
