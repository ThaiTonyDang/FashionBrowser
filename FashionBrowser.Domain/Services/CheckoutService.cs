using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.ViewModels;

namespace FashionBrowser.Domain.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderService _orderService;
        public CheckoutService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> CreateCheckoutAsync(CheckoutItemViewModel checkout, string token)
        {           
            var cartItems = checkout.CartViewModel.ListCartItem;
            var order = checkout.OrderItem;
            var discount = checkout.Discount;
            var orderDetails = await GetOrderDetails(cartItems, order.Id, discount);

            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                IsPaid = order.IsPaid,
                ShipAddress = order.ShipAddress,
                TotalPrice = order.TotalPrice,
                UserId = order.UserId,
                OrderDetails = orderDetails
            };

            var iSuccessOrder = await _orderService.CreateOrder(orderDto, token);        
            if (iSuccessOrder)
               return true;                  
            return false;
        }

        private Task<List<OrderDetailDto>> GetOrderDetails(List<CartItemViewModel> cartItems, Guid OrderId, double discount)
        {
            var orderDetails = new List<OrderDetailDto> { };
            
          
            if (cartItems != null)
            {
                foreach (var cart in cartItems)
                {
                    var orderDetailDto = new OrderDetailDto {
                        OrderId = OrderId,
                        ProductId = cart.ProductId,
                        Price = cart.Product.Price,
                        Quantity = cart.Quantity,
                        Discount = discount
                    };
                    
                    orderDetails.Add(orderDetailDto);
                }
            }
            return Task.FromResult(orderDetails);
        }
    }
}
