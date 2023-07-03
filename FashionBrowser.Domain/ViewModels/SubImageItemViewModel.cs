using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.ViewModels
{
    public class SubImageItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
    }
}
