using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionBrowser.Domain.ViewModels
{
	public class UserItemViewModel
	{
        public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Name { get => this.LastName + " " + this.FirstName; }
		public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "UPLOAD IMAGE IS REQUIRED ")]
        public IFormFile File { get; set; }
		public string Address { get; set; }
		public string AvatarImage { get; set; }
		public string ImageUrl { get; set; }
        public PasswordItemViewModel PasswordItemViewModel { get; set; }       
    }
}
