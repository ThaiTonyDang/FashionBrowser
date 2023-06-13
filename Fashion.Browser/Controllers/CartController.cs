using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using FashionBrowser.Domain.Services;
using System.Collections.Generic;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;

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

        [Authorize]
        [HttpGet]
        [Route("shoppingcart")]
        public async Task<IActionResult> Index()
        {
            var cartView = new CartViewModel();
            var claim = User.FindFirst("token");
            if (claim != null)
            {
                var token = claim.Value;
                cartView = await _cartServices.GetCartViewModel(token);
            }    

            return View(cartView);
        }

        [HttpPost]
        [Route("cart/addtocart/{productId}")]
        public async Task<IActionResult> AddToCart(string productId, int quantityInput = 0)
        {
            var claim = User.FindFirst("token");
            if (claim == null)
            {
                return Unauthorized();
            }

            var token = claim.Value;
            var tuple = await _productServices.GetProductByIdAsync(productId);
            var product = tuple.Item1;

            var cartItem = new CartItemViewModel()
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Quantity = 1,
                ProductId = product.Id,
                Product = product
            };
            var result = await _cartServices.AddToCart(cartItem, token);

            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);
            if (cartItems == null) cartItems = new List<CartItemViewModel>();

            var cartView = await _cartServices.GetCartViewModel(token);
            cartItems = cartView.ListCartItem;

            var isSuccess = result.Item1;
            if (isSuccess) return Ok(cartItems);
                return BadRequest();
        }

        [HttpDelete]
        [Route("/delete/{productId}")]
        public async  Task<IActionResult> DeleteToCart(string productId)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var token = User.FindFirst("token")?.Value;
            var result = await _cartServices.DeleteCartItem(productId, token);
            var isSuccess = result.Item1;
            var message = result.Item2;

            if (isSuccess)
            {
                return Ok();
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
