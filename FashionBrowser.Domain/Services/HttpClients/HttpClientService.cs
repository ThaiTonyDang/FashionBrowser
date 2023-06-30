using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model.Responses;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace FashionBrowser.Domain.Services.HttpClients
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public HttpClientService(IOptions<ApiConfig> options, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiUrl = $"{options.Value.Url}/api";
        }

        public async Task<ResultDto> GetAsync(string pathApiUrl)
        {
            var url = $"{_apiUrl}/{pathApiUrl}";
            var response = await _httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseApi>(jsonString);
                return new SuccessResult(result.Message);
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi>(jsonString);
            return new ErrorResult(string.Join(", ",error.Errors));
        }

        public async Task<ResultDto> GetDataAsync<TResult>(string pathApiUrl)
        {
            var url = $"{_apiUrl}/{pathApiUrl}";
            var response = await _httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseDataApi<TResult>>(jsonString);
                return new SuccessDataResult<TResult>(result.Message, result.Data);
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi>(jsonString);
            return new ErrorResult(string.Join(", ", error.Errors));
        }

        public async Task<ResultDto> PostAsync<TBody>(TBody body, string pathApiUrl, string contentType = "application/json")
        {
            var url = $"{_apiUrl}/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseApi>(jsonString);
                return new SuccessResult(result.Message);
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi>(jsonString);
            return new ErrorResult(string.Join(", ", error.Errors));
        }

        public async Task<ResultDto> PostDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string contentType = MediaTypeNames.Application.Json)
        {
            var url = $"{_apiUrl}/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseDataApi<TResult>>(jsonString);
                return new SuccessDataResult<TResult>(result.Message, result.Data);
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi>(jsonString);
            return new ErrorResult(string.Join(", ", error.Errors));
        }
    }
}
