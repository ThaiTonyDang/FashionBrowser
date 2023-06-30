using FashionBrowser.Domain.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public interface IFileServices
    {
        public Task<List<string>> GetResponeUploadFileAsync(IFormFile file, HttpClient httpClient, string token);
    }
}
