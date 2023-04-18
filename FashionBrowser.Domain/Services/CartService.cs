using FashionBrowser.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
	public class CartService : ICartServices
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

		public CartViewModel GetCartViewModel(List<CartItemViewModel> carts)
		{
			var cartViewModel = new CartViewModel();
			cartViewModel.ListCartItem = carts;
			return cartViewModel;
		}
	}
}
