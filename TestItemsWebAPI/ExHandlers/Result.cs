namespace TestItemsWebAPI.ExHandlers
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        protected Result(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true);
        public static Result Failure(string errorMessage) => new Result(false, errorMessage);
    }

    public class Result<T> : Result
    {
        public T Data { get; }

        private Result(bool isSuccess, T data, string errorMessage = null) : base(isSuccess, errorMessage)
        {
            Data = data;
        }

        public static new Result<T> Success(T data) => new Result<T>(true, data);
        public static new Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
    }
}
