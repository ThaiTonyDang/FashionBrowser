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
        public string ShipAddress{ get; set; }
        public bool IsPaid { get; set; }
        public string IsPaidDisplay { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ProductListPrice { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
    }
}
