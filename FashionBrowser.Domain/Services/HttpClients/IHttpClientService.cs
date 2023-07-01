using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services.HttpClients
{
    public interface IHttpClientService
    {
        Task<ResultDto> GetAsync(string pathApiUrl, string token = "");
        Task<ResultDto> GetDataAsync<TResult>(string pathApiUrl, string token = "");
        Task<ResultDto> PostAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PostDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PatchAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PatchDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PutAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PutDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> UploadAsync<TResult>(MultipartFormDataContent file, string pathApiUrl, string token = "");
    }
}
