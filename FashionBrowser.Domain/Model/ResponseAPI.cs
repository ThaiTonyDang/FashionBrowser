using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Model
{
    public class ResponseAPI<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string[] ErrorsDetail { get; set; }
        public T Data { get; set; }
    }
}
