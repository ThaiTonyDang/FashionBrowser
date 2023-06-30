using Fashion.Browser.Libraries;
using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections;
using System.Net.Http;
using System.Collections.Generic;

namespace Fashion.Browser.VpayServices
{
    public class VnpayServices : IVnpayServices
    {
        private readonly OrderCallbackConfig _orderCallbackConfig;
        private readonly VnpayConfig _vnpayConfig;

        public VnpayServices(IOptions<OrderCallbackConfig> orderConfig, IOptions<VnpayConfig> vnpayConfig)
        {
            _orderCallbackConfig = orderConfig.Value;
            _vnpayConfig = vnpayConfig.Value;
        }
        public Task<string> CreatePaymentUrl(CheckoutItemViewModel model, HttpContext context)
        {
            var timeNow = DateTime.Now;
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnpayLibrary();
            var urlCallBack = _orderCallbackConfig.ReturnUrl;
            var price = (((int)model.CartViewModel.Total * 100) * (1 - (1.1)/100)).ToString();

            pay.AddOrderData(model);
            pay.AddRequestData("vnp_Version", _vnpayConfig.Version);
            pay.AddRequestData("vnp_Command", _vnpayConfig.Command);
            pay.AddRequestData("vnp_TmnCode", _vnpayConfig.TmnCode);
            pay.AddRequestData("vnp_Amount", $"{price}");
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _vnpayConfig.CurrCode);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _vnpayConfig.Locale);
            pay.AddRequestData("vnp_OrderInfo",
            $"{model.OrderItem.Id} \r\n {model.UserItemViewModel.FirstName} \r\n {model.UserItemViewModel.LastName} \r\n " +
            $" {model.OrderItem.OrderDate.ToString("yyyyMMddHHmmss")} \r\n {model.OrderItem.ShipAddress} \r\n" +
            $"{(model.CartViewModel.Total) * decimal.Parse((1 - (1.1) / 100).ToString())}" +
            $" \r\n {model.OrderItem.UserId}");
            pay.AddRequestData("vnp_OrderType", "fashion");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_vnpayConfig.BaseUrl, _vnpayConfig.HashSecret);

            return Task.FromResult(paymentUrl);
        }

        public Task<PaymentResponse> PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnpayLibrary();
            var response = pay.GetFullResponseData(collections, _vnpayConfig.HashSecret);

            return Task.FromResult(response);
        }

        private Task<List<OrderDetailDto>> GetOrderDetails(List<CartItemViewModel> cartItems, Guid OrderId, double discount)
        {
            var orderDetails = new List<OrderDetailDto> { };


            if (cartItems != null)
            {
                foreach (var cart in cartItems)
                {
                    var orderDetailDto = new OrderDetailDto
                    {
                        OrderId = OrderId,
                        ProductId = cart.ProductId,
                        Price = cart.Product.Price,
                        Quantity = cart.Quantity,
                        Discount = discount
                    };

                    orderDetails.Add(orderDetailDto);
                }
            }
            return Task.FromResult(orderDetails);
        }
    }
}
