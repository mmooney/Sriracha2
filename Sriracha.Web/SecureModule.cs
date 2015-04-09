using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sriracha.Web
{
    public abstract class SecureModule : NancyModule
    {
        public SecureModule()
        {
            this.RequiresAuthentication();
        }
    }
}