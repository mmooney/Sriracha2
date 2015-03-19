using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sriracha.DeployTask.WebApplication.DeployWebApplication
{
    public class DeployWebApplicationTaskConfig
    {
        [Required]
        public string SourceWebsitePath { get; set; }

        [DefaultValue("localhost")]
        public string TargetMachineName { get; set; }

        [DefaultValue("DefaultAppPool")]
        public string ApplicationPoolName { get; set; }

        [DefaultValue("Default Web Site")]
        public string SiteName { get; set; }

        public string TargetMachinePassword { get; set; }

        public string TargetMachineUserName { get; set; }

        [Required]
        public string TargetWebsitePath { get; set; }

        public string VirtualDirectoryName { get; set; }

        [DefaultValue(true)]
        public bool DeleteTargetBeforeDeploy { get; set; }

        public Dictionary<string, string> AppSettingValues { get; set; }
        public Dictionary<string, string> ConnectionStringValues { get; set; }
        public Dictionary<string, string> XpathValues { get; set; }
    }
}
