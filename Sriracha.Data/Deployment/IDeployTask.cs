using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Deployment
{
    public interface IDeployTask
    {
        Type GetConfigType();

        void Run(TaskExecutionContext context, object config);
    }
}
