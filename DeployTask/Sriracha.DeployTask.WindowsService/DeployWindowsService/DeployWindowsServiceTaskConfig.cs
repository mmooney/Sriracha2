using dropkick.Wmi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sriracha.DeployTask.WindowsService.DeployWindowsService
{
    public class DeployWindowsServiceTaskConfig
    {
        [DefaultValue("localhost")] public string TargetMachineName { get; set; }
        [Required] public string TargetServiceDirectory { get; set; }
        public string TargetMachineUserName { get; set; }
        public string TargetMachinePassword { get; set; }

        [Required] public string ServiceName { get; set; }
        [Required] public string ServiceUserName { get; set; }
        public string ServiceUserPassword { get; set; }
        [DefaultValue(ServiceStartMode.Automatic)] public ServiceStartMode ServiceStartMode { get; set; }
        public List<string> ServiceDependencies { get; set; }

        [Required]public string SourceServiceDirectory { get; set; }
        public string SourceExeConfigPath { get; set; }

        [DefaultValue(true)] public bool DeleteTargetBeforeDeploy { get; set; }

        public Dictionary<string, string> AppSettingValues { get; set; }
        public Dictionary<string, string> ConnectionStringValues { get; set; }
        public Dictionary<string, string> XpathValues { get; set; }

    }
}
