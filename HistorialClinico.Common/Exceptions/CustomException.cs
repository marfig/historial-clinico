using System;
using System.Net;

namespace HistorialClinico.Common.Exceptions
{
    public class CustomException : Exception
    {
        public HttpStatusCode ErrorCode = 0;

        public CustomException()
            : base()
        {
        }

        public CustomException(string message, HttpStatusCode? code = null)
            : base(message)
        {
            ErrorCode = code ?? HttpStatusCode.InternalServerError;
        }

        public CustomException(string message, Exception inner, HttpStatusCode code)
            : base(message, inner)
        {
            ErrorCode = code;
        }
    }
}
