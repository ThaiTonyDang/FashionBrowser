using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
	public class CartItemViewModel
	{
		public string UserId { get; set; }
		public int Quantity { set; get; }
		public decimal Price { get => this.Quantity * this.Product.Price; }
		public Guid ProductId { get; set; }
		public ProductItemViewModel Product { set; get; }
	}

	public class CartViewModel
	{
        public bool IsSuccess { get; set; }
        public string[] ErrorDetail { get; set; }
        public List<CartItemViewModel> ListCartItem { get; set; }
		public decimal Total { get => GetTotalPrice(); }
		public CartViewModel()
		{
			this.ListCartItem = new List<CartItemViewModel>();
		}

		public decimal GetTotalPrice()
		{
			decimal total = 0;
			foreach (var cartitem in this.ListCartItem)
			{
				total += cartitem.Price;
			}

			return total;
		}
	}
}
