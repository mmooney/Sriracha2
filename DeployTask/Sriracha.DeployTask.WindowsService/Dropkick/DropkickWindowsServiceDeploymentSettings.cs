using dropkick.Configuration;
using dropkick.Wmi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sriracha.DeployTask.WindowsService.Dropkick
{
    public class DropkickWindowsServiceDeploymentSettings : DropkickConfiguration
    {
        public string TargetMachineUserName { get; set; }
        public string TargetMachinePassword { get; set; }
        [Required] public string TargetServiceDirectory { get; set; }

        [Required] public string ServiceName { get; set; }
        public string ServiceExeName { get; set; }
        [Required] public string ServiceUserName { get; set; }
        public string ServiceUserPassword { get; set; }
        [DefaultValue(ServiceStartMode.Automatic)] public ServiceStartMode ServiceStartMode { get; set; }
        public string ServiceDependencies { get; set; }
        
        [Required] public string SourceServiceDirectory { get; set; }
        public string SourceExeConfigPath { get; set; }

        [DefaultValue(true)] public bool DeleteTargetBeforeDeploy { get; set; }

        public string AppSettingValuesJson { get; set; }
        public string ConnectionStringValuesJson { get; set; }
        public string XpathValuesJson { get; set; }

    }
}
