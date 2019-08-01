using System;

namespace Estim8.Backend.Commands.Exceptions
{
    public class DomainException : Exception
    {
        public int ErrorCode { get; set; }

        public DomainException(ErrorCode errorCode, string message = null, Exception innerException = null) : base(message, innerException)
        { 
        }
    }

    public enum ErrorCode : int
    {
        GameNotFound = 1000,
        InputNotAllowed = 2000,
        NotAuthorized = 3000,
    }
}