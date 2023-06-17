using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionBrowser.Domain.ViewModels
{
	public class UserItemViewModel
	{
		[Required(ErrorMessage = "First name is required")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		public string LastName { get; set; }

		public string Name { get => this.LastName + " " + this.FirstName; }

		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }

        [Required(ErrorMessage = "NUMBER DEPARTMENT IS REQUIRED")]
        public string NumberDepartment { get; set; }

        [Required]
        public string Password { get; set; }

		[Required]
        public string ConfirmPassword { get; set; }

		public string PhoneNumber { get; set; }
		public string Address { get => GetAddress(); }
        public string AvailableAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? CityId { get; set; }
		public string? DistrictId { get; set; }
		public string? WardId { get; set; }

		public string CityName { get; set; }
		public string DistrictName { get; set; }
		public string WardName { get; set; }

		public string GetAddress()
		{
            if (string.IsNullOrEmpty(this.WardName) && string.IsNullOrEmpty(this.DistrictName))
			{
                return this.NumberDepartment + " - " + this.CityName;
            }
            if (string.IsNullOrEmpty(this.WardName))
			{
                return this.NumberDepartment + " - " + this.DistrictName + " - " + this.CityName;
            }
            if (string.IsNullOrEmpty(this.DistrictName))
            {
                return this.NumberDepartment + " - " + this.WardName + " - " + this.CityName;
            }
            return this.NumberDepartment + " - " + this.WardName + " - " + this.DistrictName + " - " + this.CityName;
		}
	}
}
