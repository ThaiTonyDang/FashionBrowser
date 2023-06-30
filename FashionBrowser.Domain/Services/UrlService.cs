using FashionBrowser.Domain.Config;
using FashionBrowser.Utilities;
using Microsoft.Extensions.Options;

namespace FashionBrowser.Domain.Services
{
    public class UrlService : IUrlService
    {
        private readonly ApiConfig _hostAPIConfig;
        public UrlService(IOptions<ApiConfig> options)
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
