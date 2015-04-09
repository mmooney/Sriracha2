using Nancy;
using Nancy.Security;
using Sriracha.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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