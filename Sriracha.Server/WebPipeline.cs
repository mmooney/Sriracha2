using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Owin;
using Sriracha.Ioc;

namespace Sriracha.Server
{
    public class WebPipeline
    {
        public void Configuration(IAppBuilder application)
        {
            //UseFileServer(application);
            //UseWebApi(application);
            application.UseNancy(options => 
                {
                    options.Bootstrapper = SrirachaIocProvider.GetNancyBootstrapper(new ServicePathProvider());
                    options.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound); //https://github.com/NancyFx/Nancy/wiki/How-to-use-System.Web.Optimization-Bundling-with-Nancy
                }
            );

            Sriracha.Web.App_Start.BundleConfig.Start();
        }
    }
}
