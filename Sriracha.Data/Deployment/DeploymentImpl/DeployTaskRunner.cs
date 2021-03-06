﻿using Common.Logging;
using Newtonsoft.Json;
using Sriracha.Data.Ioc;
using Sriracha.Data.Validation;
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
        private readonly IDeployConfigurationValidator _validator;

        public DeployTaskRunner(IIocFactory iocFactory, IDeployConfigurationValidator validator)
        {
            _iocFactory = iocFactory;
            _validator = validator;
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
                //If the working directory is not the EXE directory, it might not find the DLL in the current directory.
                //  So try to explicitly check the EXE directory
                var exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var tempTaskBinaryPath = Path.Combine(exeDirectory, Path.GetFileName(taskBinaryPath));
                if(File.Exists(tempTaskBinaryPath))
                {
                    taskBinaryPath = tempTaskBinaryPath;
                }
            }
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
                taskType = assembly.GetTypes().FirstOrDefault(i=>i.Name == taskName);
            }
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
                    throw new FileNotFoundException("configFile not found: " + configFile);
                }
                string data = File.ReadAllText(configFile);
                configObject = JsonConvert.DeserializeObject(data, configType);

                configObject = _validator.ValidateAndApplyDefaults(configObject);
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
