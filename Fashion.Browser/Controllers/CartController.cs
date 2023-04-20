using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using FashionBrowser.Domain.Services;
using System.Collections.Generic;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using System.Net.WebSockets;

namespace Fashion.Browser.Controllers
{
	public class CartController : Controller
	{
		private readonly IProductServices _productServices;
		private readonly ICartServices _cartServices;
		public CartController(IProductServices productServices, ICartServices cartServices)
		{
			_productServices = productServices;
			_cartServices = cartServices;
		}

		[Route("shoppingcart")]
		public IActionResult Index()
		{
			var cartView = new CartViewModel();
			var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);
			if (cartItems != default(List<CartItemViewModel>))
			cartView.ListCartItem = cartItems;

			return View(cartView);
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(string productid, int quantityInput = 0)
		{
			var product = await _productServices.GetProductByIdAsync(new Guid(productid));	

			if (product.UnitsInStock ==  0)
			{
				TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
				TempData[Mode.LABEL_CONFIRM_FAIL] = "THIS PRODUCT IS OUT OF STOCK";
				return RedirectToAction("Index", "Home");
			}	

			var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);

			if (cartItems == null) cartItems = new List<CartItemViewModel>();

			_cartServices.AddToCart(product, cartItems, quantityInput);
			HttpContext.Session.SetObjectAsJson(CartKeyName.Cart_Key, cartItems);

			TempData[Mode.MODE] = Mode.USING_MODAL_CONFIRM;
			TempData[Mode.MODAL_CONFIRM_SUCCESS] = "ADD THIS PRODUCT SUCCESSFULLY";
			return RedirectToAction("Index", "Home");
		}
	}
}
