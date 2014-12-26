using Common.Logging;
using Sriracha.Data.Deployment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WebApplication.DeployWebApplication
{
    public class DeployWebApplicationTask : IDeployTask
    {
        private readonly ILog _log;

        public DeployWebApplicationTask(ILog log)
        {
            _log = log;
        }

        public Type GetConfigType()
        {
            return typeof(DeployWebApplicationTaskConfig);
        }


        public void Run(object config)
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
            _log.Info("Run... " + typedConfig.ToJson(true));
        }
    }
}
