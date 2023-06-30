namespace FashionBrowser.Domain.Dto
{

    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public SuccessResult ToSuccessResult()
        {
            return new SuccessResult(this.Message);
        }

        public ErrorResult ToErrorResult()
        {
            return new ErrorResult(this.Message);
        }

        public SuccessDataResult<T> ToSuccessDataResult<T>()
        {
            return (SuccessDataResult<T>)this;
        }
    }

    public class ErrorResult : ResultDto
    {
        public ErrorResult(string message)
        {
            this.IsSuccess = false;
            this.Message = message;
        }
    }
    public class SuccessResult : ResultDto
    {
        public SuccessResult(string message)
        {
            this.IsSuccess = true;
            this.Message = message;
        }
    }

    public class SuccessDataResult<T> : ResultDto
    {
        public T Data { get; set; }
        public SuccessDataResult(string message, T data)
        {
            this.IsSuccess = true;
            this.Message = message;
            Data = data;
        }
    }
}
