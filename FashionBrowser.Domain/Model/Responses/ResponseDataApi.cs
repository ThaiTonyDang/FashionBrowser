using System.Text.Json.Serialization;

namespace FashionBrowser.Domain.Model.Responses
{
    public class ResponseDataApi<T> : BaseReponseApi
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
