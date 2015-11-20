using Sriracha.Data.Deployment;
using Sriracha.DeployTask.Helper.Dropkick;
using Sriracha.DeployTask.WindowsService.Dropkick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WindowsService.DeployWindowsService
{
    public class DeployWindowsServiceTask : IDeployTask
    {
        private readonly IDropkickRunner _dropkickRunner;

        public DeployWindowsServiceTask(IDropkickRunner dropkickRunner)
        {
            _dropkickRunner = dropkickRunner;
        }

        public Type GetConfigType()
        {
            return typeof(DeployWindowsServiceTaskConfig);
        }

        public void Run(TaskExecutionContext context, object config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (!(config is DeployWindowsServiceTaskConfig))
            {
                throw new ArgumentException("config is not DeployWindowsServiceTaskConfig");
            }
            var typedConfig = (DeployWindowsServiceTaskConfig)config;
            var deployment = new DropkickWindowsServiceDeployment();
            using (var dropkickContext = _dropkickRunner.Create(context))
            {
                var serverMap = deployment.GetDefaultServerMap();
                serverMap["WindowsService"] = typedConfig.TargetMachineName;
				var settings = new DropkickWindowsServiceDeploymentSettings
				{
					TargetMachineUserName = typedConfig.TargetMachineUserName,
					TargetMachinePassword = typedConfig.TargetMachinePassword,
					TargetServiceDirectory = typedConfig.TargetServiceDirectory,
					ServiceName = typedConfig.ServiceName,
					ServiceUserName = typedConfig.ServiceUserName,
					ServiceUserPassword = typedConfig.ServiceUserPassword,
					ServiceStartMode = typedConfig.ServiceStartMode,
					ServiceDependencies = (typedConfig.ServiceDependencies != null && typedConfig.ServiceDependencies.Any())
												? string.Join(";", typedConfig.ServiceDependencies)
												: null,
					SourceServiceDirectory = System.IO.Path.GetFullPath(typedConfig.SourceServiceDirectory),
                    SourceExeConfigPath = typedConfig.SourceExeConfigPath,
                    DeleteTargetBeforeDeploy = typedConfig.DeleteTargetBeforeDeploy,
                    AppSettingValuesJson = ToDictionaryJson(typedConfig.AppSettingValues),
                    ConnectionStringValuesJson = ToDictionaryJson(typedConfig.ConnectionStringValues),
                    XpathValuesJson = ToDictionaryJson(typedConfig.XpathValues),
					ExecutingRole = "WindowsService"
				};
                dropkickContext.Run<DropkickWindowsServiceDeployment>(settings, serverMap, "WindowsService".ListMe());

                context.Info("Done DeployWindowsServiceTask");

                //return context.BuildResult();
            }
        }

        private string ToDictionaryJson(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            else
            {
                return dictionary.ToJson();
            }
        }
    }
}
