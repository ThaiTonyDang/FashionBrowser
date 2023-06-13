using FashionBrowser.Domain.ViewModels;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
	public interface ICartServices
	{
		public Task<Tuple<bool, string[]>> AddToCart(CartItemViewModel cartItemViewModel, string token);
        public bool DeleteCartItems(List<CartItemViewModel> carts, Guid id);
		public void AdjustQuantity(CartItemViewModel cart, string operate);
		public Task<CartViewModel> GetCartViewModel(string token);
		public Task<List<CartItemViewModel>> GetCartItems(string token);
    }
}