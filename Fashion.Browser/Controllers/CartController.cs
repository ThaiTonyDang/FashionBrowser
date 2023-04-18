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
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(string productid, int quantityInput = 0)
		{
			var product = await _productServices.GetProductByIdAsync(new Guid(productid));
		

			var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);
			if (cartItems == null)
			{
				List<CartItemViewModel> cart = new List<CartItemViewModel>();
				cart.Add(new CartItemViewModel {  Quantity = 1, ProductItemViewModel = product });
				HttpContext.Session.SetObjectAsJson(CartKeyName.Cart_Key, cart);
			}	
			else
			{
				_cartServices.AddToCart(product, cartItems, quantityInput);			
			}

			TempData["StatusMessage"] = "ADD PRODUCTS SUCCESS!";
			return RedirectToAction("Index", "Home");
		}
	}
}
