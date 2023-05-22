using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
    public class CustomerItemViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "NAME DEPARTMENT IS REQUIRED")]
        public string Name { get; set; }
        [Required(ErrorMessage = "PHONE DEPARTMENT IS REQUIRED")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "EMAIL DEPARTMENT IS REQUIRED")]
        public string Email { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Ward { get; set; }
        [Required(ErrorMessage = "NUMBER DEPARTMENT IS REQUIRED")]
        public string NumberDepartment { get; set; }
        public bool RegistrationStatus { get; set; }
        public string Address { get; set; }

        public string GetAddress()
        {
            if (string.IsNullOrEmpty(this.County) || string.IsNullOrEmpty(this.Ward) || string.IsNullOrEmpty(this.City))
            {
                return this.NumberDepartment + " " +  "Đà NẴNG";
            }

            return NumberDepartment + " " + Ward + " " + County + " " + City;
        }
    }
}
