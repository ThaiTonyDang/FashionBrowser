using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
    public class OrderItemViewModel
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string ShipAddress{ get; set; }
        public string Status { get; set; }
        public Guid CustomerId { get; set; }

        public OrderItemViewModel()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
