using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sriracha.DeployTask.WebApplication.DeployWebApplication
{
    public class DeployWebApplicationTaskConfig
    {
        public string TargetMachineName { get; set; }

        public string ApplicationPoolName { get; set; }

        public string SiteName { get; set; }

        public string TargetMachinePassword { get; set; }

        public string TargetMachineUserName { get; set; }

        public string TargetWebsitePath { get; set; }

        public string VirtualDirectoryName { get; set; }
    }
}
