namespace Core.ResultPattern
{
    public class Result<TValue>
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }
        public TValue? Value { get; }

        private Result(TValue value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(Error error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static implicit operator Result<TValue>(TValue value) => new Result<TValue>(value);

        public static implicit operator Result<TValue>(Error error) => new Result<TValue>(error);
    }
}
