﻿using Common.Logging;
using Sriracha.Data.Deployment;
using Sriracha.DeployTask.Helper.Dropkick;
using Sriracha.DeployTask.WebApplication.Dropkick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WebApplication.DeployWebApplication
{
    public class DeployWebApplicationTask : IDeployTask
    {
        private readonly IDropkickRunner _dropkickRunner;

        public DeployWebApplicationTask(IDropkickRunner dropkickRunner)
        {
            _dropkickRunner = dropkickRunner;
        }

        public Type GetConfigType()
        {
            return typeof(DeployWebApplicationTaskConfig);
        }


        public void Run(TaskExecutionContext context, object config)
        {
            if(config == null)
            {
                throw new ArgumentNullException("config");
            }
            if(!(config is DeployWebApplicationTaskConfig))
            {
                throw new ArgumentException("config is not DeployWebApplicationTaskConfig");
            }
            var typedConfig = (DeployWebApplicationTaskConfig)config;
            var deployment = new DropkickWebDeployment();
            using (var dropkickContext = _dropkickRunner.Create(context))
            {
                var serverMap = deployment.GetDefaultServerMap();
                serverMap["Website"] = typedConfig.TargetMachineName;
                var settings = new DropkickWebDeploymentSettings
                {
                    SourceWebsitePath = System.IO.Path.GetFullPath(typedConfig.SourceWebsitePath),
                    DeleteTargetBeforeDeploy = typedConfig.DeleteTargetBeforeDeploy,
                    ApplicationPoolName = typedConfig.ApplicationPoolName,
                    Environment = "ENV",
                    Role = "Website",
                    SiteName = typedConfig.SiteName,
                    TargetMachinePassword = typedConfig.TargetMachinePassword,
                    TargetMachineUserName = typedConfig.TargetMachineUserName,
                    TargetWebsitePath = typedConfig.TargetWebsitePath,
                    VirtualDirectoryName = typedConfig.VirtualDirectoryName,
                    AppSettingValuesJson = ToDictionaryJson(typedConfig.AppSettingValues),
                    ConnectionStringValuesJson = ToDictionaryJson(typedConfig.ConnectionStringValues),
                    XpathValuesJson = ToDictionaryJson(typedConfig.XpathValues)
                };
                dropkickContext.Run<DropkickWebDeployment>(settings, serverMap, "Website".ListMe());

                context.Info("Done DeployWebsiteTask");

                //return context.BuildResult();
            }
        }

        private string ToDictionaryJson(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }
            else 
            {
                return dictionary.ToJson();
            }
        }
    }
}
