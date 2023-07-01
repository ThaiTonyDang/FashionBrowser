using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FashionBrowser.Domain.ViewModels.Users
{
    public class UserLoginViewModel
    {
        public string Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }
}
