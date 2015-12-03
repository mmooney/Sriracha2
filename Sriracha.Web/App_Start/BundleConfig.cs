using Sriracha.Web.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;

//[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BundleConfig), "Start")]
//[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebFormNinject.App_Start.NinjectWebCommon), "Stop")]

namespace Sriracha.Web.App_Start
{
    public class BundleConfig
    {
        public static void Start()
        {
            Start_Squishit();
        }

        public static void Start(string webPath)
        {
            Start_Squishit(webPath);
        }

        public static void Start_Squishit(string webPath=null)
        { 
            if(!string.IsNullOrEmpty(webPath))
            {
                Environment.CurrentDirectory = webPath; //SquishIt assumes that that the current directory is the web directory because ... ?
            }

            SquishIt.Framework.Bundle.JavaScript()
                .Add("~/content/scripts/sriracha-app/templates/sriracha-app-templates.js")
                .WithMinifier<SquishIt.Framework.Minifiers.JavaScript.MsMinifier>()
                .AsCached("sriracha-app-templates", string.Format("~/bundles/js/sriracha-app-templates"));

            SquishIt.Framework.Bundle.JavaScript()
                .Add("~/content/scripts/sriracha-app/js/sriracha-app.js")
                .AddDirectory("~/content/scripts/sriracha-app/js", true)
                .WithMinifier<SquishIt.Framework.Minifiers.JavaScript.MsMinifier>()
                .AsCached("sriracha-app-scripts", string.Format("~/bundles/js/sriracha-app-scripts"));

            //SquishIt.Framework.Bundle.JavaScript()
            //    .Add(SetPath("~/content/scripts/sriracha-app/templates/sriracha-app-templates.js", webPath))
            //    .AsCached("~/sriracha-app/templates",SetPath("~/bundles/js/sriracha-app-templates.js", webPath));
            //SquishIt.Framework.Bundle.JavaScript()
            //    .Add(SetPath("~/content/scripts/sriracha-app/js/sriracha-app.js", webPath))
            //    .AddDirectory(SetPath("~/content/scripts/sriracha-app/js",webPath), true)
            //    .AsCached("~/sriracha-app/scripts", SetPath("~/bundles/js/sriracha-app.js", webPath));

        }

        private static string SetPath(string path, string webPath)
        {
            //if(!string.IsNullOrEmpty(webPath))
            //{
            //    return Path.GetFullPath(Path.Combine(webPath, path.Replace("~/", "./")));
            //}
            //else 
            //{
                return path;
            //}
        }

        public static void Start_SystemWebOptimatization()
        {
            var bundles = BundleTable.Bundles;

            bundles.Add(new Bundle("~/sriracha-app/templates"/*, new HTMLBarsTransformer()*/)
                            .Include("~/content/scripts/sriracha-app/templates/sriracha-app-templates.js")
                            //.IncludeDirectory("~/content/scripts/sriracha-app/templates", "*.hbs.js", true))
                    );

            bundles.Add(new ScriptBundle("~/sriracha-app/scripts")
                        .Include("~/content/scripts/sriracha-app.js")
                        .IncludeDirectory("~/content/scripts/sriracha-app/js", "*.js", true));

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}