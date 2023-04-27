using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using FashionBrowser.Domain.Services;
using System.Collections.Generic;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Routing;
using System.Linq;

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

            if (product.UnitsInStock == 0)
            {
                return BadRequest("OUT OF STOCK");
            }
            var cartItems = session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);
            if (cartItems == null) cartItems = new List<CartItemViewModel>();

            _cartServices.AddToCart(product, cartItems, quantityInput);
            HttpContext.Session.SetObjectAsJson(CartKeyName.Cart_Key, cartItems);

            return Ok(cartItems);
        }

        [HttpDelete]
        [Route("/delete/{productId}")]
        public IActionResult DeleteToCart(string productId)
        {
            var session = HttpContext.Session;
            var carts = session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);

            if (carts == null) return StatusCode(404);

            var result = productId.IsGuidParseFromString();
            if (result)
            {
                var isSucces = _cartServices.DeleteCartItems(carts, new Guid(productId));
                if (isSucces)
                {
                    HttpContext.Session.SetObjectAsJson(CartKeyName.Cart_Key, carts);

                    return Ok(carts);
                }

            }

            return BadRequest();
        }


        [HttpPost]
        [Route("/adjustquantity/{productId}/{operate}")]
        public IActionResult AdjustQuantity(string operate, string productId)
        {
            var session = HttpContext.Session;
            var carts = session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);

            if (carts == null) return StatusCode(404);

            var result = productId.IsGuidParseFromString();

            if (result)
            {
                var id = new Guid(productId);
                var cartItem = carts.Where(cart => cart.Product.Id == id).FirstOrDefault();

                if (cartItem != null)
                {
                    _cartServices.AdjustQuantity(cartItem, operate);
                    HttpContext.Session.SetObjectAsJson(CartKeyName.Cart_Key, carts);
                    return Ok(carts);
                }

                return StatusCode(401);
            }

            return BadRequest();
        }
    }
}
