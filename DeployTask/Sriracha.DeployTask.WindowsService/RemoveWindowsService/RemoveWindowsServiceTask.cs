using Sriracha.Data.Deployment;
using Sriracha.DeployTask.Helper.Dropkick;
using Sriracha.DeployTask.WindowsService.Dropkick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WindowsService.RemoveWindowsService
{
    public class RemoveWindowsServiceTask : IDeployTask
    {
        private readonly IDropkickRunner _dropkickRunner;

        public RemoveWindowsServiceTask(IDropkickRunner dropkickRunner)
        {
            _dropkickRunner = dropkickRunner;
        }

        public Type GetConfigType()
        {
            return typeof(RemoveWindowsServiceTaskConfig);
        }

        public void Run(TaskExecutionContext context, object config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (!(config is RemoveWindowsServiceTaskConfig))
            {
                throw new ArgumentException("config is not RemoveWindowsServiceTask");
            }
            var typedConfig = (RemoveWindowsServiceTaskConfig)config;
            var deployment = new DropkickWindowsServiceDeployment();
            using (var dropkickContext = _dropkickRunner.Create(context))
            {
                var serverMap = deployment.GetDefaultServerMap();
                serverMap["RemoveWindowsService"] = typedConfig.TargetMachineName;
                var settings = new DropkickWindowsServiceDeploymentSettings
                {
                    TargetServiceDirectory = typedConfig.TargetServiceDirectory,
                    ServiceName = typedConfig.ServiceName,

					ExecutingRole = "RemoveWindowsService"
				};
                dropkickContext.Run<DropkickWindowsServiceDeployment>(settings, serverMap, "RemoveWindowsService".ListMe());

                context.Info("Done RemoveWindowsServiceTask");

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
