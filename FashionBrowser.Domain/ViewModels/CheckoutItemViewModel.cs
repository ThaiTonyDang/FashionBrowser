using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
    public class CheckoutItemViewModel
    {
        public CustomerItemViewModel CustomerItemViewModel { get; set; }
        public CartViewModel CartViewModel { get; set; }
    }
}
