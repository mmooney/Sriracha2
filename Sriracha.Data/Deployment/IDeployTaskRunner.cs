using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Deployment
{
    public interface IDeployTaskRunner
    {
        void RunTask(IDeployStatusReporter statusReporter, string taskBinary, string taskName, string configFile, string workingDirectory);
    }
}
