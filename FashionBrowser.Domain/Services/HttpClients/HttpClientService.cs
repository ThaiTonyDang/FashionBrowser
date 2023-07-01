using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.Model.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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

        public async Task<ResultDto> GetAsync(string pathApiUrl, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/{pathApiUrl}";
            var response = await _httpClient.GetAsync(url);
            return await this.GetResponseAsync(response);
        }

        public async Task<ResultDto> GetDataAsync<TResult>(string pathApiUrl, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/{pathApiUrl}";
            var response = await _httpClient.GetAsync(url);
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public async Task<ResultDto> PostAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }
            var url = $"{_apiUrl}/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseAsync(response);
        }

        public async Task<ResultDto> PostDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public async Task<ResultDto> PatchAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PatchAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseAsync(response);
        }

        public async Task<ResultDto> PatchDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PatchAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public async Task<ResultDto> PutAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PutAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseAsync(response);
        }

        public async Task<ResultDto> PutDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PutAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public async Task<ResultDto> UploadAsync<TResult>(MultipartFormDataContent file, string pathApiUrl, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/{pathApiUrl}";
            var response = await _httpClient.PostAsync(url, file);
            return await this.GetResponseDataAsync<TResult>(response);
        }

        private async Task<ResultDto> GetResponseAsync(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseApi>(jsonString);
                return new SuccessResult(result.Message);
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi>(jsonString);
            return new ErrorResult(string.Join(", ", error.Errors));
        }

        private async Task<ResultDto> GetResponseDataAsync<TResult>(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseDataApi<TResult>>(jsonString);
                return new SuccessDataResult<TResult>(result.Message, result.Data);
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi>(jsonString);
            return new ErrorResult(string.Join(", ", error.Errors));
        }

        private void SetAuthenticationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
        }
    }
}
