using Common.Logging;
using MMDB.Azure.Management;
using MMDB.Azure.Management.AzureDto.AzureCloudService;
using MMDB.Azure.Management.AzureDto.AzureStorage;
using Sriracha.Data.Deployment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.Azure.DeployCloudService
{
    public class DeployCloudServiceTask : IDeployTask
    {
        public DeployCloudServiceTask()
        {
        }

        public Type GetConfigType()
        {
            return typeof(DeployCloudServiceTaskConfig);
        }

        public void Run(TaskExecutionContext context, object config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (!(config is DeployCloudServiceTaskConfig))
            {
                throw new ArgumentException("config is not DeployCloudServiceTaskConfig");
            }
            var typedConfig = (DeployCloudServiceTaskConfig)config;

            var client = new AzureClient(typedConfig.AzureSubscriptionIdentifier, typedConfig.AzureManagementCertificate);
            var service = client.GetCloudService(typedConfig.ServiceName);
            //var list = client.GetCloudServiceList();

            ////var existingService = computeManagementClient.HostedServices.Get(formattedServiceName);
            //var service = list.FirstOrDefault(i=>i.ServiceName == formattedServiceName);
            if (service == null)
            {
                context.Info("Service {0} does not yet exist, creating...", typedConfig.ServiceName);

                string message;
                if (!client.CheckCloudServiceNameAvailability(typedConfig.ServiceName, out message))
                {
                    throw new ArgumentException(string.Format("Service name {0} not available: {1}", typedConfig.ServiceName, message));
                }
                client.CreateCloudService(typedConfig.ServiceName);
                service = client.GetCloudService(typedConfig.ServiceName);
                context.Info("Service {0} created successfully", typedConfig.ServiceName);
            }
            else
            {
                context.Info("Service {0} already exists", typedConfig.ServiceName);
            }

            var storageAccount = client.GetStorageAccount(typedConfig.StorageAccountName);
            if (storageAccount == null)
            {
                context.Info("Storage Account {0} does not exist, creating...", typedConfig.StorageAccountName);

                string message;
                if (!client.CheckStorageAccountNameAvailability(typedConfig.StorageAccountName, out message))
                {
                    throw new ArgumentException(string.Format("Storage Account name {0} not available: {1}", typedConfig.StorageAccountName, message));
                }
                client.CreateStorageAccount(typedConfig.StorageAccountName);
                storageAccount = client.GetStorageAccount(typedConfig.StorageAccountName);
            }
            else
            {
                context.Info("Storage Account {0} exists", typedConfig.StorageAccountName);
            }

            storageAccount = client.WaitForStorageAccountStatus(typedConfig.StorageAccountName, StorageServiceProperties.EnumStorageServiceStatus.Created, TimeSpan.FromMinutes(typedConfig.AzureTimeoutMinutes));

            var keys = client.GetStorageAccountKeys(typedConfig.StorageAccountName);

            context.Info("Uploading Azure package file to blog storage: {0}", typedConfig.AzurePackagePath);
            var blobUrl = client.UploadBlobFile(typedConfig.StorageAccountName, keys.Secondary, typedConfig.AzurePackagePath, "srirachadeploy");

            string configurationData = File.ReadAllText(typedConfig.AzureConfigPath);

            DeploymentItem deployment = null;
            if (service.DeploymentList != null)
            {
                deployment = service.DeploymentList.FirstOrDefault(i => typedConfig.DeploymentSlot.Equals(i.DeploymentSlot, StringComparison.CurrentCultureIgnoreCase));
            }
            if (deployment == null)
            {
                context.Info("Deployment does not yet exist, creating...");
                client.CreateCloudServiceDeployment(typedConfig.ServiceName, blobUrl, configurationData, typedConfig.DeploymentSlot);
            }
            else
            {
                context.Info("Deployment already exists, upgrading...");
                client.UpgradeCloudServiceDeployment(typedConfig.ServiceName, blobUrl, configurationData, typedConfig.DeploymentSlot);
            }
            deployment = client.WaitForCloudServiceDeploymentStatus(typedConfig.ServiceName, typedConfig.DeploymentSlot, DeploymentItem.EnumDeploymentItemStatus.Running, TimeSpan.FromMinutes(typedConfig.AzureTimeoutMinutes));
            deployment = client.WaitForAllCloudServiceInstanceStatus(typedConfig.ServiceName, typedConfig.DeploymentSlot, RoleInstance.EnumInstanceStatus.ReadyRole, TimeSpan.FromMinutes(typedConfig.AzureTimeoutMinutes));
            //_logger.Info("Done DeployCloudService.InternalExecute");
            //return context.BuildResult();
        }
    }
}
