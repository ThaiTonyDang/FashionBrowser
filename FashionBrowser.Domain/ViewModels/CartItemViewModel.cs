using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
	public class CartItemViewModel
	{
		public int Quantity { set; get; }
		public decimal Price { get => this.Quantity * this.Product.Price; }
		public ProductItemViewModel Product { set; get; }
	}

	public class CartViewModel
	{
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
