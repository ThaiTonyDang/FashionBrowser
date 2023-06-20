using FashionBrowser.Domain.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    
    public class FileService : IFileService
    {
        private readonly IUrlService _urlService;
        public FileService(IUrlService urlService)
        {
            _urlService = urlService;
        }
        public async Task<List<string>> GetResponeUploadFileAsync(IFormFile file, HttpClient httpClient, string token)
        {
            var uploadApiUrl = _urlService.GetBaseUrl() + "/api/File/upload";
            var fileName = file.FileName;
            var content = new MultipartFormDataContent();

            content.Add(new StreamContent(file.OpenReadStream())
            {
                Headers =
                    {
                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                }
            }, "File", fileName);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                 JwtBearerDefaults.AuthenticationScheme, token);
            var response = await httpClient.PostAsync(uploadApiUrl, content);
            var listData = JsonConvert.DeserializeObject<ResponseAPI<List<string>>>
                            (await response.Content.ReadAsStringAsync());
            return listData.Data;
        }
    }
}
