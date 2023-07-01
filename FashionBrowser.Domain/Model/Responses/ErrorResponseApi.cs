using System.Text.Json.Serialization;

namespace FashionBrowser.Domain.Model.Responses
{
    public class ErrorResponseApi : BaseReponseApi
    {
        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }
    }
}
