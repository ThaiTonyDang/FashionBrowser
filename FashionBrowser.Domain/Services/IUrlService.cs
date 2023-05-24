using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public interface IUrlService
    {
        public string GetBaseUrl();
        public string GetFileApiUrl(string fileName);
    }
}
