﻿using Common.Logging;
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
        private readonly ILog _log;
        private readonly IIocFactory _iocFactory;

        public DeployTaskRunner(ILog log, IIocFactory iocFactory)
        {
            _log = log;
            _iocFactory = iocFactory;
        }

        public void RunTask(string taskBinary, string taskName, string configFile)
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
            _log.Info("Loading task binary " + taskBinaryPath);
            var assembly = Assembly.LoadFile(taskBinaryPath);

            _log.Info("Getting type " + taskName);
            var taskType = assembly.GetType(taskName);

            if(!typeof(IDeployTask).IsAssignableFrom(taskType))
            {
                throw new ArgumentException("Type " + taskType.FullName + " does not implement interface " + typeof(IDeployTask).FullName);
            }

            _log.Info("Registering assembly types from " + taskBinaryPath);
            _iocFactory.RegisterAssembly(assembly);

            _log.Info("Instantiating type " + taskType.FullName);
            var taskObject = (IDeployTask)_iocFactory.Get(taskType);

            _log.Info("HI!");

        }
    }
}
