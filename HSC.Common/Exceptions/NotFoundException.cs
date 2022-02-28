using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HSC.Common.Exceptions
{
    public class NotFoundException : HttpResponseException
    {
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message)
        {
        }
    }
}
