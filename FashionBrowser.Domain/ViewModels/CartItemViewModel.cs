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
		public ProductItemViewModel Product { set; get; }
	}

	public class CartViewModel
	{
		public List<CartItemViewModel> ListCartItem { get; set; }
		public CartViewModel()
		{
			this.ListCartItem = new List<CartItemViewModel>();
		}
	}
}
