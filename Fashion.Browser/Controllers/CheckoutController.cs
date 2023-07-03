using Fashion.Browser.VpayServices;
using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fashion.Browser.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutServices _checkoutService;
        private readonly ICartServices _cartServices;
        private readonly IVnpayServices _vnpayServices;
        private readonly IUrlServices _urlServices;
        private static CheckoutItemViewModel _staticCheckoutItem;

        public CheckoutController(ICheckoutServices checkoutService,
            ICartServices cartServices, IVnpayServices vnpayServices, IUrlServices urlServices)
        {
            _checkoutService = checkoutService;
            _cartServices = cartServices;
            _vnpayServices = vnpayServices;
            _urlServices = urlServices;
        }

        [HttpGet]
        [Route("checkout/order-information")]
        public async Task<IActionResult> Index()
        {
            var checkout = new CheckoutItemViewModel();
            var cartItems = await GetCartList();

            checkout.CartViewModel.ListCartItem = cartItems;
            checkout.UserItemViewModel = new UserItemViewModel
            {
                FirstName = User.Claims.FirstOrDefault(x => x.Type == "firstName")?.Value,
                LastName = User.Claims.FirstOrDefault(x => x.Type == "lastName")?.Value,
                Email = User.FindFirstValue(ClaimTypes.Email),
                PhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone), 
                Address = User.FindFirstValue(ClaimTypes.StreetAddress), 
            };
            if (cartItems == null) cartItems = new List<CartItemViewModel>();
            return View(checkout); // index thông tin 
        }

        [Route("checkout/confirm-order/")]
        public async Task<IActionResult> ConfirmOrder(CheckoutItemViewModel checkout)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var cartItems = await GetCartList();
            if (checkout.CartViewModel == null || cartItems.Count == 0)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "There Are Currently No Orders! Shopping Now !";
                return RedirectToAction("Index", "Home");
            }

            if (checkout.UserItemViewModel != null)
            {
                var address = checkout.UserItemViewModel.Address;
                checkout.CartViewModel.ListCartItem = cartItems;

                checkout.OrderItem = BuidOrder(address);
                if (!checkout.IsCardCreditPay)
                {
                    checkout.OrderItem.IsPaid = false;
                    checkout.OrderItem.IsPaidDisplay = "Customer Pays After Receiving Goods";
                    checkout.OrderItem.Status = "COD";
                }

                checkout.OrderItem.IsPaidDisplay = "Paid Via VNPAY Gateway";
                checkout.OrderItem.IsPaid = true;
                _staticCheckoutItem = checkout;
            }                        
            checkout = _staticCheckoutItem;
            if (_staticCheckoutItem == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "There Are Currently No Orders! Shopping Now !";
                return RedirectToAction("Index", "Home");
            }
      
            return View(checkout);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(CheckoutItemViewModel checkout)
        {
            checkout = _staticCheckoutItem;        
            var url = await _vnpayServices.CreatePaymentUrl(checkout, HttpContext);
            return Redirect(url);                  
        }

        [Route("checkout/create-order/")]
        public async Task<IActionResult> CreateOrder(CheckoutItemViewModel checkout)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var token = User.FindFirst("token").Value;
            var cartItems = await GetCartList();
            checkout.CartViewModel.ListCartItem = cartItems;
            if (checkout.CartViewModel == null || cartItems.Count == 0 && _staticCheckoutItem == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "There Are Currently No Orders! Shopping Now !";
                return RedirectToAction("Index", "Home");
            }

            if (checkout.UserItemViewModel != null && checkout.OrderItem != null)
            {
                var isSuccess = await _checkoutService.CreateCheckoutAsync(checkout, token);
                var isSuccessRemove = await RemoveItemCart(token);
                if (isSuccess && isSuccessRemove)
                {
                    TempData[Mode.LABEL_CONFIRM_SUCCESS] = "Payment success !";
                    _staticCheckoutItem = checkout;
                    return View(checkout);
                }
            }

            checkout = _staticCheckoutItem;
            if (_staticCheckoutItem == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "There Are Currently No Orders! Shopping Now !";
                return RedirectToAction("Index", "Home");
            }
            return await Task.Run(() => View(checkout));
        }

        [Route("vnpay/order-information")]
        public async Task<IActionResult> VnpayCreateOrder()
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var token = User.FindFirst("token").Value;
            var response = await _vnpayServices.PaymentExecute(Request.Query);
            if (!response.Success)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "Payment Fail ! Try Again";
                return RedirectToAction("Index", "Checkout");
            }
            var checkout = _staticCheckoutItem;

            foreach (var cart in checkout.CartViewModel.ListCartItem)
            {
                cart.Product.ImageUrl = _urlServices.GetFileApiUrl(cart.Product.MainImageName);
            }

            if (checkout.OrderItem == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "No Orders Yet! Please Check Your Email";
                return View();
            }
            var isSuccess = await _checkoutService.CreateCheckoutAsync(checkout, token);
            var isSuccessRemove = await RemoveItemCart(token);
            if (!isSuccess)
            {
                TempData[Mode.LABEL_CONFIRM_FAIL] = "Payment Fail ! Try Again";
                return RedirectToAction("Index", "Checkout");
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
