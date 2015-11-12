using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace GeocachingExample.Models
{
    public class GeocacheException : Exception
    {
        public HttpStatusCode ResopnseCode { get; set; }
        //public string Message { get; set; }

        public GeocacheException(HttpStatusCode responseCode, string message)
            : base(message)//responseCode)
        {
            ResopnseCode = responseCode;
        }
    }
}