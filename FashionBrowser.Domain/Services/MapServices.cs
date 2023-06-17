using FashionBrowser.Domain.Model;
using FashionBrowser.Domain.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public class MapServices : IMapServices
    {
        private readonly HttpClient _httpClient;
        public MapServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<City>> GetCities()
        {
            var apiUrl = "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json";
            var response = await _httpClient.GetAsync(apiUrl);
            var cities = JsonConvert.DeserializeObject<City[]>
                                  (await response.Content.ReadAsStringAsync());

            return cities.ToList();
        }
    }
}
