using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Owin;
using Sriracha.Ioc;
using MMDB.Shared;
using System.IO;

namespace Sriracha.Server
{
    public class WebPipeline
    {
        public void Configuration(IAppBuilder application)
        {
            string webPath = Path.GetFullPath(AppSettingsHelper.GetRequiredSetting("WebPath"));
            //UseFileServer(application);
            //UseWebApi(application);
            application.UseNancy(options => 
                {
                    options.Bootstrapper = SrirachaIocProvider.GetNancyBootstrapper(new ServicePathProvider(webPath));
                    options.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound); //https://github.com/NancyFx/Nancy/wiki/How-to-use-System.Web.Optimization-Bundling-with-Nancy
                }
            );

            Sriracha.Web.App_Start.BundleConfig.Start(webPath);
        }
    }
}
