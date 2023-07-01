using System.ComponentModel.DataAnnotations;

namespace FashionBrowser.Domain.ViewModels
{
    public class RegisterItemViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PHONE IS REQUIRED")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "NUMBER DEPARTMENT IS REQUIRED")]
        public string NumberDepartment { get; set; }

        public string Address { get => GetAddress(); }

        [Required(ErrorMessage = "City")]
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }

        [Required(ErrorMessage = "PASSWORD IS REQUIRED ")
        , DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "CONFIRM PASSWORD IS REQUIRED ")
        , DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm password does not match")]
        public string ConfirmPassword { get; set; }

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

            return this.NumberDepartment + " - " + this.WardName + " - " + this.WardName + " - " + this.CityName;
        }


    }
}
