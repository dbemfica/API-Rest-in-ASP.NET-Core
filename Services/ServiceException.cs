using System;

namespace startapidotnet.Services
{
    public class ServiceException : Exception
    {
        public string message { get; }
        public int code { get; }

        public ServiceException(string message, int code){
            this.message = message;
            this.code = code;
        }
    }
}
