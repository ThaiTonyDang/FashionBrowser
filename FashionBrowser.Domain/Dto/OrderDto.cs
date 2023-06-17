using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipAddress { get; set; }
        public bool IsPaid { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public ICollection<OrderDetailDto> OrderDetails { get; set; }
    }
}
