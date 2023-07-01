using System.Text.Json.Serialization;

namespace FashionBrowser.Domain.Model.Files
{
    public class FileUpload
    {
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("fileLink")]
        public string FileLink { get; set; }
    }
}
