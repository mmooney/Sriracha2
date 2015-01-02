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
        private readonly ILog _log;
        private readonly IDropkickRunner _dropkickRunner;

        public DeployWebApplicationTask(ILog log, IDropkickRunner dropkickRunner)
        {
            _log = log;
            _dropkickRunner = dropkickRunner;
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
