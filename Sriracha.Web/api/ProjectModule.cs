using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sriracha.Web.api
{
    public class ProjectModule : NancyModule
    {
        private Sriracha.Data.Utility.IZipper _zipper;

        public ProjectModule(Sriracha.Data.Utility.IZipper zipper) : base("api/project")
        {
            _zipper = zipper;

            Get["/"] = _ => "Hello Projects!: " + _zipper.GetType().FullName;
        }
    }
}