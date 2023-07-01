using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model.Files;
using FashionBrowser.Domain.Services.HttpClients;

namespace FashionBrowser.Domain.Services
{
    public class FileService : IFileService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiResource = "file";
        public FileService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<ResultDto> UploadFileAsync(MultipartFormDataContent file, string token)
        {
            var url = $"{_apiResource}/upload";
            var response = await _httpClientService.UploadAsync<FileUpload>(file, url, token);
            return response;
        }

    }
}
