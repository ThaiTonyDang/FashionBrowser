using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FashionBrowser.Domain.ViewModels.Users
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get => LastName + " " + FirstName; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Upload image is required ")]
        public IFormFile File { get; set; }
        public string Address { get; set; }
        public string AvatarImage { get; set; }
        public string ImageUrl { get; set; }
        public UserPasswordViewModel UserPasswordViewModel { get; set; }
    }
}
