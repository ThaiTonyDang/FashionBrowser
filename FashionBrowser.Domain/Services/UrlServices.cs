using FashionBrowser.Domain.Config;
using FashionBrowser.Utilities;
using Microsoft.Extensions.Options;

namespace FashionBrowser.Domain.Services
{
    public class UrlServices : IUrlServices
    {
        private readonly APIConfig _hostAPIConfig;
        public UrlServices(IOptions<APIConfig> options)
        {
            _hostAPIConfig = options.Value;
        }
        public string GetBaseUrl()
        {
            return _hostAPIConfig.Url;
        }

        public string GetFileApiUrl(string fileName)
        {
            var fileUrl = GetBaseUrl() + $"/{HTTP.SLUG}/" + fileName;
            return fileUrl;
        }
    }
}
