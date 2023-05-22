using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Fashion.Browser.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        public CheckoutController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var checkout = new CheckoutItemViewModel();
            checkout.CustomerItemViewModel = new CustomerItemViewModel();
            checkout.CartViewModel = new CartViewModel();
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
                var cartItems = GetCartList();
                var customer = checkout.CustomerItemViewModel;
                var orderDetail = BuidOrderDetail(cartItems);
                var order = BuidOrder(checkout.CustomerItemViewModel, orderDetail);

                var result = await _orderService.CreateOrder(order);
                var isSuccess = result.Item1;
                message = result.Item2;
                TempData[Mode.MODE] = Mode.USING_MODAL_CONFIRM;
                if (isSuccess)
                {
                    TempData[Mode.MODAL_CONFIRM_SUCCESS] = message;
                    return View();
                }
            }
          
            TempData[Mode.MODAL_CONFIRM_FAIL] = message;
            return RedirectToAction("Index", "Checkout");
        }

        private OrderDetailItemViewModel BuidOrderDetail(List<CartItemViewModel> cartItemViewModels)
        {
            var orderDetail = new OrderDetailItemViewModel();  
            orderDetail.CartItemViewModels = cartItemViewModels.Select(c => c).ToList() ;

            return orderDetail;
        }

        private OrderItemViewModel BuidOrder(CustomerItemViewModel customer, OrderDetailItemViewModel orderDetail)
        {
            var order = orderDetail.OrderItemViewModel;
            order.OrderDate = DateTime.Now;
            order.RequiredDate = DateTime.Now.AddDays(ADDDATE.EXPIREDATE);
            order.CustomerId = customer.Id;
            order.ShipAddress = customer.Address;

            return order;
        }

        private List<CartItemViewModel> GetCartList()
        {
            var session = HttpContext.Session;
            var cartItems = session.GetObjectFromJson<List<CartItemViewModel>>(CartKeyName.Cart_Key);
            return cartItems;
        }
    }
}
