using FashionBrowser.Domain.Config;
using FashionBrowser.Utilities;
using Microsoft.Extensions.Options;

namespace FashionBrowser.Domain.Services
{
    public class UrlService : IUrlService
    {
        private readonly ApiConfig _hostAPIConfig;
        private readonly string _apiUrl;
        public UrlService(IOptions<ApiConfig> options)
        {
            _apiUrl = options.Value.Url;
            _hostAPIConfig = options.Value;
        }
        public string GetBaseUrl()
        {
            return _hostAPIConfig.Url;
        }

        public string GetFileApiUrl(string fileName)
        {
            var fileUrl = $"{_apiUrl}/{HTTP.SLUG}/" + fileName;
            return fileUrl;
        }
    }
}
