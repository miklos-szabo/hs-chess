using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HSC.Common.Exceptions
{
    public class HttpResponseException : Exception
    {
        public virtual HttpStatusCode StatusCode { get; set; }

        public HttpResponseException(string message) : base(message)
        {
        }
    }
}
