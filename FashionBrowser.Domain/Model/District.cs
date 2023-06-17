using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Model
{
    public class District
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string WardId { get; set; }
        public ICollection<Ward> Wards { get; set; }
    }
}
