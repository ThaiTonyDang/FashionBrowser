using Fashion.Browser.VpayServices;
using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public CheckoutController(ICheckoutServices checkoutService,
            ICartServices cartServices, IVnpayServices vnpayServices, IUrlServices urlServices)
        {
            _checkoutService = checkoutService;
            _cartServices = cartServices;
            _vnpayServices = vnpayServices;
            _urlServices = urlServices;
        }

        [HttpGet]
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
             
        public async Task<IActionResult> ConfirmOrder(CheckoutItemViewModel checkout)
        {          
            var address = checkout.UserItemViewModel.Address;
            var cartItems = await GetCartList();   
          
            checkout.CartViewModel.ListCartItem = cartItems;

            checkout.OrderItem = BuidOrder(address);
            if (!checkout.IsCardCreditPay)
            {
                checkout.OrderItem.IsPaid = false;
                checkout.OrderItem.IsPaidDisplay = "Customer Pays After Receiving Goods";
                checkout.OrderItem.Status = "COD";                       
            }

            checkout.OrderItem.IsPaidDisplay = "Paid Via VNPAY Gateway";
            return View(checkout);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(CheckoutItemViewModel checkout)
        {
            var token = User.FindFirst("token").Value;
            var cartItems = await GetCartList();

            checkout.CartViewModel.ListCartItem = cartItems;
            var isSuccess = await _checkoutService.CreateCheckoutAsync(checkout, token);
   
            if (isSuccess)
            {
                var url = await _vnpayServices.CreatePaymentUrl(checkout, HttpContext);
                return Redirect(url);
            }
            
            return RedirectToAction(" ConfirmOrder", "Checkout");
        }

        public async Task<IActionResult> CreateOrder(CheckoutItemViewModel checkout)
        {
            TempData[Mode.MODE] = Mode.USING_LABEL_CONFIRM;
            var token = User.FindFirst("token").Value;
            var cartItems = await GetCartList();
            checkout.CartViewModel.ListCartItem = cartItems;
            var isSuccess = await _checkoutService.CreateCheckoutAsync(checkout, token);
            var isSuccessRemove = await RemoveItemCart(token);
            if (isSuccess && isSuccessRemove)
            {
                TempData[Mode.LABEL_CONFIRM_SUCCESS] = "Payment success !";
                return View(checkout);
            }
            return await Task.Run(() => View(checkout));
        }

        [Route("orders/order-information")]
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
            string[] data = response.OrderDescription.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            var checkout = GetDataFromResponseVnpay(data);

            var cartItems = await GetCartList();
            checkout.CartViewModel.ListCartItem = cartItems;
            foreach (var cart in checkout.CartViewModel.ListCartItem)
            {
                cart.Product.ImageUrl = _urlServices.GetFileApiUrl(cart.Product.MainImageName);
            }

            if (checkout.OrderItem == null)
            {
                TempData[Mode.LABEL_CONFIRM_CHECK] = "No Orders Yet! Please Check Your Email";
                return View();
            }

            TempData[Mode.LABEL_CONFIRM_SUCCESS] = "Payment success !";
            var orderDto = new OrderDto
            {
                Id = checkout.OrderItem.Id,
            };

            var update = _checkoutService.UpdatePaidStatus(orderDto, token);
            var isSuccessRemove = await RemoveItemCart(token);
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

        private CheckoutItemViewModel GetDataFromResponseVnpay(string[] data)
        {
            var checkout = new CheckoutItemViewModel();
            if (!data[0].IsGuidParseFromString()) { data[0] = Guid.NewGuid().ToString(); }
            if (!data[3].IsParseDateTime()) { data[3] = DateTime.Now.ToString(); }
            if (!data[6].IsGuidParseFromString()) { data[6] = Guid.NewGuid().ToString(); }
            checkout.OrderItem = new OrderItemViewModel();
            checkout.UserItemViewModel = new UserItemViewModel();
            checkout.OrderItem.Id = new Guid(data[0]);
            checkout.UserItemViewModel.FirstName = data[1];
            checkout.UserItemViewModel.LastName = data[2];
            checkout.OrderItem.OrderDate = DateTime.Parse(data[3]);
            checkout.OrderItem.ShipAddress = data[4];
            checkout.OrderItem.TotalPrice = decimal.Parse(data[5]);
            checkout.OrderItem.UserId = new Guid(data[6]);
            checkout.OrderItem.IsPaidDisplay = "Customer Paid Via Credit Card";
            checkout.OrderItem.IsPaid = true;

            return checkout;
        }
    }
}
