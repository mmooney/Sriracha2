using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sriracha.Server
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override IRootPathProvider RootPathProvider
        {
            get { return new NancyPathProvider(); }
        }
        
    }
}
