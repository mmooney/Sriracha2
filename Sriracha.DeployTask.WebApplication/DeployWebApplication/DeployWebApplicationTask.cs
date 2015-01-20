using Common.Logging;
using Sriracha.Data.Deployment;
using Sriracha.DeployTask.WebApplication.Dropkick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WebApplication.DeployWebApplication
{
    public class DeployWebApplicationTask : IDeployTask
    {
        private readonly IDropkickRunner _dropkickRunner;

        public DeployWebApplicationTask(IDropkickRunner dropkickRunner)
        {
            _dropkickRunner = dropkickRunner;
        }

        public Type GetConfigType()
        {
            return typeof(DeployWebApplicationTaskConfig);
        }


        public void Run(IDeployStatusReporter statusReporter, object config)
        {
            if(config == null)
            {
                throw new ArgumentNullException("config");
            }
            if(!(config is DeployWebApplicationTaskConfig))
            {
                throw new ArgumentException("config is not DeployWebApplicationTaskConfig");
            }
            var typedConfig = (DeployWebApplicationTaskConfig)config;
            statusReporter.Info("Run... " + typedConfig.ToJson(true));
        }
    }
}
