using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
    public class OrderDetailItemViewModel
    {
        public Guid OrderId { get; set; }
        public OrderItemViewModel OrderItemViewModel { get; set; }
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public ICollection<CartItemViewModel> CartItemViewModels{ get; set; }
        public OrderDetailItemViewModel()
        {
            this.OrderItemViewModel = new OrderItemViewModel();
        }
    }
}
