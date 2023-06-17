using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
    public class CheckoutItemViewModel
    {
        public UserItemViewModel UserItemViewModel { get; set; }
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
