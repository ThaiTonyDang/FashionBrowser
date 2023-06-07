using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
          
        public IActionResult CreateOrder(CheckoutItemViewModel checkout)
        {
            TempData[Mode.MODE] = Mode.USING_MODAL_CONFIRM;
            if (ModelState.IsValid)
            {
                var customer = checkout.CustomerItemViewModel;
                customer.Id = Guid.NewGuid();
                var cartItems = GetCartList();
                
                checkout.CartViewModel.ListCartItem = cartItems;
                checkout.CustomerItemViewModel = customer;
                checkout.OrderItem = BuidOrder(customer);

                return View(checkout);
            }
          
            TempData[Mode.MODAL_CONFIRM_FAIL] = "Cannot Create Order! Try Again";
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

        public async Task<IActionResult> Confirm(CheckoutItemViewModel checkout)
		{
            TempData[Mode.MODE] = Mode.USING_MODAL_CONFIRM;
            if (checkout.OrderItem == null)
            {
                TempData[Mode.MODAL_CONFIRM] = "No Orders Yet! Please Check Your Email";
                return RedirectToAction("Index", "Home");
            }
            var cartItems = GetCartList();
            checkout.CartViewModel.ListCartItem = cartItems;
            checkout.OrderItem.IsPaidDispay = "Customer Pays After Receiving Goods";

            if (checkout.IsCardCreditPay)
            {
                checkout.OrderItem.IsPaidDispay = "Customer Paid Via Credit Card";
                checkout.OrderItem.IsPaid = true;
            }    

            var isSuccess = await _checkoutService.CreateCheckout(checkout);
            if (isSuccess)
            {
                RemoveItemCart();
                TempData[Mode.MODAL_CONFIRM_SUCCESS] = "Payment success !";
                return View(checkout);
            }
                                                 
            TempData[Mode.MODAL_CONFIRM_FAIL] = "Payment success Fail !";
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
			order.CustomerId = customer.Id;
			order.ShipAddress = customer.Address;

			return order;
		}

        private void RemoveItemCart()
        {
            var session = HttpContext.Session;
            session.Clear();
        }
    }
}
