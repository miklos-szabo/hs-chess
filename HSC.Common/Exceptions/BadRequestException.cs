using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HSC.Common.Exceptions
{
    public class BadRequestException : HttpResponseException
    {
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

        public BadRequestException(string message) : base(message)
        {
        }
    }
}
