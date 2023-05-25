using FashionBrowser.Domain.Config;
using FashionBrowser.Utilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public class UrlService : IUrlService
    {
        private readonly APIConfig _hostAPIConfig;
        public UrlService(IOptions<APIConfig> options)
        {
            _hostAPIConfig = options.Value;
        }
        public string GetBaseUrl()
        {
            return _hostAPIConfig.Url;
        }

        public string GetFileApiUrl(string fileName)
        {
            var fileUrl = GetBaseUrl() + HTTP.SLUG + fileName;
            return fileUrl;
        }
    }
}
