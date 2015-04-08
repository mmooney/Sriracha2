﻿using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Sriracha.Ioc;

namespace Sriracha.Server
{
    public class WebPipeline
    {
        public void Configuration(IAppBuilder application)
        {
            //UseFileServer(application);
            //UseWebApi(application);
            application.UseNancy(options => options.Bootstrapper = SrirachaIocProvider.GetNancyBootstrapper(new ServicePathProvider()));
        }
    }
}
