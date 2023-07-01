using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Domain.ViewModels.Users;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fashion.Browser.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ICartServices _cartServices;
        private readonly IMapServices _mapServices;
        public CheckoutController(ICheckoutService checkoutService, ICartServices cartServices, IMapServices mapServices)
        {
            _checkoutService = checkoutService;
            _cartServices = cartServices;
            _mapServices = mapServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var checkout = new CheckoutItemViewModel();
            var cartItems = await GetCartList();

            checkout.CartViewModel.ListCartItem = cartItems;
            checkout.UserItemViewModel = new UserViewModel
            {
                FirstName = User.Claims.FirstOrDefault(x => x.Type == "firstName")?.Value,
                LastName = User.Claims.FirstOrDefault(x => x.Type == "lastName")?.Value,
                Email = User.FindFirstValue(ClaimTypes.Email),
                PhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone), 
                Address = User.FindFirstValue(ClaimTypes.StreetAddress), 
            };

            if (cartItems == null) cartItems = new List<CartItemViewModel>();
            return View(checkout);
        }

        [HttpPost]               
        public async Task<IActionResult> ConfirmOrder(CheckoutItemViewModel checkout)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TempData[Mode.MODE] = Mode.USING_MODAL_CONFIRM;
       
            if(!userId.IsGuidParseFromString())
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "Cannot Create Order! User Id Invalid";
                return RedirectToAction("index", "home");
            }

            var address = checkout.UserItemViewModel.Address;

            var cartItems = await GetCartList();          
            checkout.CartViewModel.ListCartItem = cartItems;
            checkout.OrderItem = BuidOrder(address);

            return View(checkout);
        }

        public async Task<IActionResult> CreateOrder(CheckoutItemViewModel checkout)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            
            var token = User.FindFirst("token").Value;
            if (checkout.OrderItem == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "No Orders Yet! Please Check Your Email";
                return View();
            }
            var cartItems = await GetCartList();
            checkout.CartViewModel.ListCartItem = cartItems;
            checkout.OrderItem.IsPaidDispay = "Customer Pays After Receiving Goods";

            if (checkout.IsCardCreditPay)
            {
                checkout.OrderItem.IsPaidDispay = "Customer Paid Via Credit Card";
                checkout.OrderItem.IsPaid = true;
            }

            var isSuccess = await _checkoutService.CreateCheckoutAsync(checkout, token);
            var isSuccessRemove = await RemoveItemCart(token);
            if (isSuccess && isSuccessRemove)
            {
                TempData[Mode.LABEL_CONFIRM_SUCCESS] = "Payment success !";
                return View(checkout);
            }

            TempData[Mode.LABEL_CONFIRM_FAIL] = "Payment success Fail !";

            return RedirectToAction("Index", "Checkout");
        }

        public IActionResult CardCredit(CheckoutItemViewModel checkout)
        {
            if (!checkout.IsCardCreditPay)
            {
                return RedirectToAction("Index", "Home");
            }    
            return View(checkout);
        }

		private async Task<List<CartItemViewModel>> GetCartList()
        {
            var token = User.FindFirst("token").Value;
            var cartItems = await _cartServices.GetCartItems(token);
            return cartItems;
        }

        private OrderItemViewModel BuidOrder(string address)
        {
			var order = new OrderItemViewModel();
			order.Id = Guid.NewGuid();
			order.OrderDate = DateTime.Now;
			order.UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
			order.ShipAddress = address;
			return order;
		}

        private async Task<bool> RemoveItemCart(string token)
        {
            var result = await _cartServices.DeleteAllCartByUser(token);
            return result;
        }
    }
}
