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
        public CustomerItemViewModel CustomerItemViewModel { get; set; }
        public CartViewModel CartViewModel { get; set; }
        public OrderItemViewModel OrderItem { get; set; }
        public bool IsCODPay { get; set; }
        public CheckoutItemViewModel()
        {
            this.CartViewModel = new CartViewModel();
            this.CustomerItemViewModel = new CustomerItemViewModel();
        }
    }
}
