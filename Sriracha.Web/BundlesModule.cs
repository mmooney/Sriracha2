using Nancy;
using SquishIt.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;

namespace Sriracha.Web
{
    public class BundlesModule : NancyModule
    {
        public BundlesModule() : base("bundles")
        {
            Get["/js/{name}"] = _ => CreateResponse(Bundle.JavaScript().RenderCached((string)_.name),
                                                    Configuration.Instance.JavascriptMimeType);
            Get["/css/{name}"] = _ => CreateResponse(Bundle.Css().RenderCached((string)_.name),
                                                    Configuration.Instance.CssMimeType);

        }

        private dynamic CreateResponse(string content, string contentType)
        {
            var response = this.Response.FromStream(()=>GZipStream.Synchronized(new MemoryStream(Encoding.UTF8.GetBytes(content))),contentType);
            if(this.Request.Query["r"] != null)
            {
                response.WithHeader("etag", (string)this.Request.Query["r"]);
            }
//#if DEBUG
//            response.WithHeader("Cache-Control", "max-age=45");
//#else
//            response.WithHeader("Cache-Control", "max-age=604800");
//#endif
            return response;
        }
    }
}