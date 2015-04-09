using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sriracha.Web
{
    public class HomeModule : SecureModule
    {
        public HomeModule() 
        {
            Get["/"] = _ => View["index"];
        }
    }
}