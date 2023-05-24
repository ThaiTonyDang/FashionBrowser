using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private Guid _id;
        public CheckoutService(ICustomerService customerService, IOrderService orderService, IOrderDetailService orderDetailService)
        {
            _customerService = customerService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }
        public async Task<bool> CreateCheckout(CheckoutItemViewModel checkout)
        {
            var customer = checkout.CustomerItemViewModel;
            var cartItems = checkout.CartViewModel.ListCartItem;
            var order = checkout.OrderItem;

            var iSuccessCustomer = await _customerService.CreateCustomerInfor(customer);
            var iSuccessOrder = await _orderService.CreateOrder(order);
            var iSuccessOrderDetail = await CreateOrderDetail(cartItems, order.Id);

            if (iSuccessCustomer)                              
                if (iSuccessOrder)
                    if (iSuccessOrderDetail)
                        return true;                  
            return false;
        }

        private async Task<bool> CreateOrderDetail(List<CartItemViewModel> cartItems, Guid OrderId)
        {
            var orderDetail = new OrderDetailItemViewModel();
            var isSuccess = false;
            orderDetail.OrderId = OrderId;
            foreach (var cart in cartItems)
            {
                orderDetail.ProductId = cart.Product.Id;
                orderDetail.Price = cart.Product.Price;
                orderDetail.Quantity = cart.Quantity;
                isSuccess = await _orderDetailService.CreateOrderDetail(orderDetail);               
            }

            return isSuccess;
        }
    }
}
