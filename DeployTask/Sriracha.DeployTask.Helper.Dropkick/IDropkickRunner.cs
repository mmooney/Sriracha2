using Sriracha.Data.Deployment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.Helper.Dropkick
{
    public interface IDropkickRunner
    {
        DropkickRunnerContext Create(TaskExecutionContext taskExecutionContext);
    }
}
