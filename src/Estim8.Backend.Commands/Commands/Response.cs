using System;

namespace Estim8.Backend.Commands.Commands
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }

        public static Response Success => new Response {IsSuccess = true};

        public static Response Failed(Exception exception = null, string errorMessage = null)
        {
            return FromResult(false, exception, errorMessage);
        }

        public static Response FromResult(bool isSuccess, Exception exception = null, string errorMessage = null)
        {
            return new Response
            {
                IsSuccess = isSuccess,
                Exception = exception,
                ErrorMessage = errorMessage
            };
        }
        
        public static Response<T> FromResult<T>(T message)
        {
            return new Response<T>
            {
                Message = message,
                IsSuccess = true
            };
        }
    }

    public class Response<T> : Response
    {
        public T Message { get; set; }
    }
}