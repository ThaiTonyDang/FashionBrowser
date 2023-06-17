using FashionBrowser.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public interface IMapServices
    {
        public Task<List<City>> GetCities();
    }
}
