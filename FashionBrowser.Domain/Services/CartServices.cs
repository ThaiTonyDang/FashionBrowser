using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FashionBrowser.Domain.Services
{
	public class CartServices : ICartServices
	{
        private readonly IUrlService _urlService;
        private readonly HttpClient _httpClient;
        public bool _isSuccess;
        public string[] _errorDetail;
        public string _message;
        public CartServices(IUrlService urlService, HttpClient httpClient)
        {
			_urlService = urlService;
			_httpClient = httpClient;
		}
		public async Task<Tuple<bool, string[]>> AddToCart(CartItemViewModel cartItemViewModel, string token)
		{
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/carts";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync(apiUrl , cartItemViewModel);

                var responseList = JsonConvert.DeserializeObject<ResponseAPI<CartItemViewModel>>
                                   (await response.Content.ReadAsStringAsync());
                _isSuccess = responseList.IsSuccess;
                _errorDetail = responseList.ErrorsDetail;
                _message = responseList.Message;

                return Tuple.Create(_isSuccess, new string[] { _message });
            }
            catch (Exception exception)
            {
                _errorDetail = new string[] { exception.Message };
                return Tuple.Create(false,  _errorDetail); ;
            }
        }

		public async Task<bool> AdjustQuantity(CartItemViewModel cartItem, string operate, string token)
		{
			switch(operate)
			{
				case OPERATOR.ADDITION:
					cartItem.Quantity++;
					break;
				default:
					cartItem.Quantity--;
					if (cartItem.Quantity < 1)
					{
						cartItem.Quantity = 1;
					}
					break;
			}

            var apiUrl = _urlService.GetBaseUrl() + "/api/carts";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync(apiUrl, cartItem);
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<bool>>
                                   (await response.Content.ReadAsStringAsync());
            var isSuccess = responseList.IsSuccess;
            return isSuccess;
        }

		public Task<CartItemViewModel> GetCartItemByProductId(List<CartItemViewModel> carts, Guid id)
		{
			var cartitem = carts.Find(item => item.ProductId == id);
			return Task.FromResult(cartitem);
        }

        public async Task<CartViewModel> GetCartViewModel(string token)
        {
            var cartViewModel = new CartViewModel();
            cartViewModel.ListCartItem = await GetCartItems(token);

            cartViewModel.IsSuccess = _isSuccess;
            cartViewModel.ErrorDetail = _errorDetail;

            return cartViewModel;
        }

        public async Task<List<CartItemViewModel>> GetCartItems(string token)
        {
			try
			{
                var apiUrl = _urlService.GetBaseUrl() + "/api/carts/";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync(apiUrl);
                var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<CartItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());
                if(responseList != null)
                {
                    _isSuccess = responseList.IsSuccess;
                    _errorDetail = responseList.ErrorsDetail;
                    var carts = responseList.Data;

                    foreach (var cartItem in carts)
                    {
                        cartItem.Product.ImageUrl = _urlService.GetFileApiUrl(cartItem.Product.MainImageName);
                    }
                    return carts;
                }

                return new List<CartItemViewModel>();
              
            }
            catch (Exception exception)
            {
                _errorDetail = new string[] { exception.Message };
                return null;
            }
        }

        public async Task<Tuple<bool, string>> DeleteCartItem(string productId, string token)
        {
            var apiUrl = _urlService.GetBaseUrl() + "/api/carts/";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync(apiUrl + productId);
            var responseList = JsonConvert.DeserializeObject<ResponseAPI<bool>>
                                   (await response.Content.ReadAsStringAsync());
            var isSuccess = responseList.IsSuccess;
            var message = responseList.Message;

            return Tuple.Create(isSuccess, message);
        }
    }
}
