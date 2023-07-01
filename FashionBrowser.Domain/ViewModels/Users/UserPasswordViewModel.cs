using System.ComponentModel.DataAnnotations;

namespace FashionBrowser.Domain.ViewModels.Users
{
    public class UserPasswordViewModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Confirm new password does not match")]
        public string ConfirmPassword { get; set; }

        public bool ValidationPassword()
        {
            if(string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
            {
                return false;
            }

            if(NewPassword != ConfirmPassword)
            {
                return false;
            }

            return true;
        }
    }
}
