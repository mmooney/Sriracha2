using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Deployment.DeploymentImpl
{
    public class LoggerDeployStatusReporter : IDeployStatusReporter
    {
        private readonly ILog _log;

        public LoggerDeployStatusReporter(ILog log)
        {
            _log = log;
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Debug(string message)
        {
            _log.Debug(message);
        }
    }
}
