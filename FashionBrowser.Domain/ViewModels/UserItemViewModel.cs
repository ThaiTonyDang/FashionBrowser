using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
	public class UserItemViewModel
	{
		[Required(ErrorMessage = "First name is required")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }

		public string UserName { get; set; }

		[Required]
        public string Password { get; set; }

		[Required]
        public string ConfirmPassword { get; set; }

		public string PhoneNumber { get; set; }
	}
}
