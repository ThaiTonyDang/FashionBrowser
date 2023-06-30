using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Fashion.Browser.VpayServices
{
    public interface IVnpayServices
    {
        public Task<string> CreatePaymentUrl(CheckoutItemViewModel checkoutModel, HttpContext context);
        public Task<PaymentResponse> PaymentExecute(IQueryCollection collections);
    }
}
