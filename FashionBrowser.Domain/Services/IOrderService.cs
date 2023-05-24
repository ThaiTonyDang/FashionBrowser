using FashionBrowser.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public interface IOrderService
    {
        public Task<bool> CreateOrder(OrderItemViewModel orderItemViewModel);
    }
}
