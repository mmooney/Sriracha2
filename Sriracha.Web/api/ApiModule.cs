using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sriracha.Web.api
{
    public class ApiModule : NancyModule
    {
        public ApiModule() : base("api")
        {
            Get["/"] = _ => 
                Response.AsJson(new 
                {
                    ProjectUrl = "api/project{/id}"
                });
        }
    }
}