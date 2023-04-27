using FashionBrowser.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
	public interface ICartServices
	{
		public void AddToCart(ProductItemViewModel product, List<CartItemViewModel> carts, int quantityInput);
		public bool DeleteCartItems(List<CartItemViewModel> carts, Guid id);
		public void AdjustQuantity(CartItemViewModel cart, string operate);
	}
}