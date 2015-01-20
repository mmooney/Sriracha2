using Sriracha.Data.Deployment;
using Sriracha.Data.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WebApplication.Dropkick.DropkickImpl
{
    public class DropkickRunner : IDropkickRunner
    {
        private readonly IZipper _zipper;
        private readonly IProcessRunner _processRunner;

        public DropkickRunner(IZipper zipper, IProcessRunner processRunner)
        {
            _zipper = zipper;
            _processRunner = processRunner;
        }

        public DropkickRunnerContext Create(TaskExecutionContext taskExecutionContext)
        {
            string dropkickDirectory = Path.Combine(taskExecutionContext.DeploymentDirectory, "Dropkick_" + DateTime.UtcNow.Ticks);
            if (!Directory.Exists(dropkickDirectory))
            {
                Directory.CreateDirectory(dropkickDirectory);
            }
            var dropkickZipName = Path.Combine(dropkickDirectory, "dropkick.zip");
            File.WriteAllBytes(dropkickZipName, DropkickResources.dropkick_zip);
            _zipper.ExtractFile(dropkickZipName, dropkickDirectory);

            return new DropkickRunnerContext(_processRunner, taskExecutionContext, dropkickDirectory);
        }
    }
}
