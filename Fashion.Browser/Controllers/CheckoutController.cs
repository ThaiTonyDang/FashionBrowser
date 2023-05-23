using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Fashion.Browser.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        public IActionResult Index()
        {
            var checkout = new CheckoutItemViewModel();
            var cartItems = GetCartList();
            checkout.CartViewModel.ListCartItem = cartItems;

            if (cartItems == null) cartItems = new List<CartItemViewModel>();
            return View(checkout);
        } 
        
        public async Task<IActionResult> Confirmation(CheckoutItemViewModel checkout)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var customer = checkout.CustomerItemViewModel;
                customer.Id = Guid.NewGuid();
                var cartItems = GetCartList();
                

                checkout.CartViewModel.ListCartItem = cartItems;
                checkout.CustomerItemViewModel = customer;
                checkout.OrderItem = BuidOrder(customer);

                var isSuccess = await _checkoutService.CreateCheckout(checkout);

                TempData[Mode.MODE] = Mode.USING_MODAL_CONFIRM;
                if (isSuccess)
                {
                    TempData[Mode.MODAL_CONFIRM_SUCCESS] = "Order Success !";
                    return View(checkout);
                }
            }
          
            TempData[Mode.MODAL_CONFIRM_FAIL] = message;
            return RedirectToAction("Index", "Checkout");
        }

        private List<CartItemViewModel> GetCartList()
        {
            var session = HttpContext.Session;
            var cartItems = session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);
            return cartItems;
        }

        private OrderItemViewModel BuidOrder(CustomerItemViewModel customer)
        {
			var order = new OrderItemViewModel();
			order.Id = Guid.NewGuid();
			order.OrderDate = DateTime.Now;
			order.RequiredDate = DateTime.Now.AddDays(ADDDATE.EXPIREDATE);
			order.CustomerId = customer.Id;
			order.ShipAddress = customer.Address;

			return order;
		}
    }
}
