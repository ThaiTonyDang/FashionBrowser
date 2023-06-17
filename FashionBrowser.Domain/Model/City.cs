using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Model
{
    public class City
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DistrictId { get; set; }
        public ICollection<District> Districts { get; set; }
    }
}
