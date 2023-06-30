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
        Task<ResultDto> GetAsync(string pathApiUrl);
        Task<ResultDto> PostDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string contentType = MediaTypeNames.Application.Json);

        Task<ResultDto> PostAsync<TBody>(TBody body, string pathApiUrl, string contentType = MediaTypeNames.Application.Json);
    }
}
