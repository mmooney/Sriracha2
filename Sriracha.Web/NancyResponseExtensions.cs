using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sriracha.Web
{
    public static class NancyResponseExtensions
    {
        public static Response AsError(this IResponseFormatter formatter, HttpStatusCode statusCode, string message)
        {
            return new Response
                {
                    StatusCode = statusCode,
                    //ContentType = "text/plain",
                    Contents = stream => (new StreamWriter(stream) { AutoFlush = true }).Write(message)
                };
        }
    }
}