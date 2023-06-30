namespace FashionBrowser.Domain.Model.Responses
{
    public class ErrorResponseApi : BaseReponseApi
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
