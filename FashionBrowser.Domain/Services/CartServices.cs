using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using System.Net.Http;

namespace FashionBrowser.Domain.Services
{
	public class CartServices : ICartServices
	{
        public CartServices()
		{

		}
		public void AddToCart(ProductItemViewModel product, List<CartItemViewModel> carts, int quantityInput)
		{
			var cartitem = carts.Find(item => item.Product.Id == product.Id);
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
				carts.Add(new CartItemViewModel() { Quantity = 1, Product = product });
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

		public void AdjustQuantity(CartItemViewModel cartItem, string operate)
		{
			switch(operate)
			{
				case OPERATOR.ADDITION:
					cartItem.Quantity++;
					break;
				default:
					cartItem.Quantity--;
					if (cartItem.Quantity < 1)
					{
						cartItem.Quantity = 1;

					}
					break;
			}
		}

		public CartItemViewModel GetCartItemByProductId(List<CartItemViewModel> carts, Guid id)
		{
			var cartitem = carts.Find(item => item.Product.Id == id);
			return cartitem;
        }
    }
}
