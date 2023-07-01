using FashionBrowser.Domain.Dto;
using Microsoft.AspNetCore.Http;

namespace FashionBrowser.Domain.Services
{
    public interface IFileService
    {
        public Task<ResultDto> UploadFileAsync(MultipartFormDataContent file, string token);
    }
}
