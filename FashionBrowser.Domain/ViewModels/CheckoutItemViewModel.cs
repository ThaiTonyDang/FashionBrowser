using FashionBrowser.Domain.ViewModels.Users;

namespace FashionBrowser.Domain.ViewModels
{
    public class CheckoutItemViewModel
    {
        public UserViewModel UserItemViewModel { get; set; }
        public CartViewModel CartViewModel { get; set; }
        public OrderItemViewModel OrderItem { get; set; }
        public bool IsCardCreditPay { get; set; }
        public double Discount { get; set; }
        public CheckoutItemViewModel()
        {
            this.CartViewModel = new CartViewModel();
        }
    }
}
