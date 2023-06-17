using FashionBrowser.Domain.ViewModels;

namespace FashionBrowser.Domain.Services
{
	public interface ICartServices
	{
		public Task<Tuple<bool, string[]>> AddToCart(CartItemViewModel cartItemViewModel, string token);
		public Task<bool> AdjustQuantity(CartItemViewModel cart, string operate, string token);
		public Task<CartViewModel> GetCartViewModel(string token);
		public Task<List<CartItemViewModel>> GetCartItems(string token);
		public Task<Tuple<bool, string>> DeleteCartItem(string productId, string token);
		public Task<CartItemViewModel> GetCartItemByProductId(List<CartItemViewModel> carts, Guid productId);
		public Task<bool> DeleteAllCartByUser(string token);
    }
}