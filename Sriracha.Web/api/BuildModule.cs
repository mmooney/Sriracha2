using Sriracha.Data.Builds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sriracha.Web.api
{
    public class BuildModule : SecureModule
    {
        private IBuildManager _buildManager;

        public BuildModule(IBuildManager buildManager)
        {
            _buildManager = buildManager;
        }
    }
}