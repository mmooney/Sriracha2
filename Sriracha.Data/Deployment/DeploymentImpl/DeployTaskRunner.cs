using Common.Logging;
using Newtonsoft.Json;
using Sriracha.Data.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Deployment.DeploymentImpl
{
    public class DeployTaskRunner : IDeployTaskRunner
    {
        private readonly IIocFactory _iocFactory;

        public DeployTaskRunner(IIocFactory iocFactory)
        {
            _iocFactory = iocFactory;
        }

        public void RunTask(IDeployStatusReporter statusReporter, string taskBinary, string taskName, string configFile, string workingDirectory)
        {
            if(string.IsNullOrEmpty(taskBinary))
            {
                throw new ArgumentNullException("taskBinary");
            }
            if(string.IsNullOrEmpty(taskName))
            {
                throw new ArgumentNullException("taskName");
            }
            string taskBinaryPath = Path.GetFullPath(taskBinary);
            if(!File.Exists(taskBinaryPath))
            {
                throw new FileNotFoundException(taskBinaryPath);
            }
            statusReporter.Debug("Loading task binary " + taskBinaryPath);
            var assembly = Assembly.LoadFrom(taskBinaryPath);

            statusReporter.Info("Getting type " + taskName);
            var taskType = assembly.GetType(taskName);

            if(taskType == null)
            {
                throw new ArgumentException("Task type " + taskName  + " not found in " + taskBinaryPath);
            }
            if(!typeof(IDeployTask).IsAssignableFrom(taskType))
            {
                throw new ArgumentException("Type " + taskType.FullName + " does not implement interface " + typeof(IDeployTask).FullName);
            }

            statusReporter.Debug("Registering assembly types from " + taskBinaryPath);
            _iocFactory.RegisterAssembly(assembly);

            statusReporter.Debug("Instantiating type " + taskType.FullName);
            var taskObject = (IDeployTask)_iocFactory.Get(taskType);

            object configObject = null;
            var configType = taskObject.GetConfigType();
            if(configType != null)
            {
                if(string.IsNullOrEmpty(configFile))
                {
                    throw new ArgumentNullException("configFile");
                }
                if(!File.Exists(configFile))
                {
                    throw new FileNotFoundException(configFile);
                }
                string data = File.ReadAllText(configFile);
                configObject = JsonConvert.DeserializeObject(data, configType);
            }

            var context = new TaskExecutionContext
            {
                StatusReporter = statusReporter,
                DeploymentDirectory = workingDirectory
            };
            taskObject.Run(context, configObject);
        }
    }
}
