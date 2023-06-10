using FashionBrowser.Domain.ViewModels;

namespace FashionBrowser.Domain.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private double _discount;
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
            var discount = checkout.Discount;

            var iSuccessCustomer = await _customerService.CreateCustomerInfor(customer);
            var iSuccessOrder = await _orderService.CreateOrder(order);
            var iSuccessOrderDetail = await CreateOrderDetail(cartItems, order.Id, discount);

            if (iSuccessCustomer)                              
                if (iSuccessOrder)
                    if (iSuccessOrderDetail)
                        return true;                  
            return false;
        }

        private async Task<bool> CreateOrderDetail(List<CartItemViewModel> cartItems, Guid OrderId, double discount)
        {
            var orderDetail = new OrderDetailItemViewModel();
            var isSuccess = false;
            orderDetail.OrderId = OrderId;
            if (cartItems != null)
            {
                foreach (var cart in cartItems)
                {
                    orderDetail.ProductId = cart.Product.Id;
                    orderDetail.Price = cart.Product.Price;
                    orderDetail.Quantity = cart.Quantity;
                    orderDetail.Discount = discount;
                    isSuccess = await _orderDetailService.CreateOrderDetail(orderDetail);
                }
            }
            return isSuccess;
        }
    }
}
