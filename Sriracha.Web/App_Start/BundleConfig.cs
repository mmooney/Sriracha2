using HTMLBarsHelper;
using Sriracha.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BundleConfig), "Start")]
//[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebFormNinject.App_Start.NinjectWebCommon), "Stop")]

namespace Sriracha.Web.App_Start
{
    public class BundleConfig
    {
        //public static void Start_Squishit()
        //{
        //    SquishIt.Framework.Bundle.JavaScript()
        //        .
        //}
        public static void Start()
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