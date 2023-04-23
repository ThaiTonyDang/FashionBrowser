using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using FashionBrowser.Domain.Services;
using System.Collections.Generic;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Routing;

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
        [Route("cart/addtocart/{productId}")]
        public async Task<IActionResult> AddToCart(string productId, int quantityInput = 0)
        {
            var session = HttpContext.Session;
            var product = await _productServices.GetProductByIdAsync(new Guid(productId));	

            if (product.UnitsInStock ==  0)
            {
                return BadRequest("OUT OF STOCK");
            }	
            var cartItems = session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);         
            if (cartItems == null) cartItems = new List<CartItemViewModel>();

            _cartServices.AddToCart(product, cartItems, quantityInput);
            HttpContext.Session.SetObjectAsJson(CartKeyName.Cart_Key, cartItems);

            return Ok(cartItems);
        }   
    }
}
