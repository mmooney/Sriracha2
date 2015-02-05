using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Deployment
{
    public class TaskExecutionContext
    {
        public string DeploymentDirectory { get; set; }
        public IDeployStatusReporter StatusReporter { get; set; }

        public void Info(string message, params object[] args)
        {
            this.StatusReporter.Info(message, args);
        }

        public string DeployCredentialsUserName { get; set; }
        public string DeployCredentialsUserPassword { get; set; }

        public string DeployCredentialsDomain { get; set; }

        public void Error(string message, params object[] args)
        {
            this.StatusReporter.Error(message, args);
        }
    }
}
