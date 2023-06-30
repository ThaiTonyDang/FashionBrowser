using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public interface ICheckoutServices
    {
        public Task<bool> CreateCheckoutAsync(CheckoutItemViewModel checkout, string token);
        public Task<bool> UpdatePaidStatus(OrderDto order, string token);
    }
}
