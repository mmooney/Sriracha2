using Sriracha.Data.Deployment;
using Sriracha.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WebApplication.Dropkick
{
    public class DropkickRunnerContext
    {
        private IProcessRunner _processRunner;
        private TaskExecutionContext _taskExecutionContext;
        private string _dropkickDirectory;

        public DropkickRunnerContext(IProcessRunner processRunner, TaskExecutionContext taskExecutionContext, string dropkickDirectory)
        {
            _processRunner = processRunner;
            _taskExecutionContext = taskExecutionContext;
            _dropkickDirectory = dropkickDirectory;
        }
    }
}
