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
using System.Xml;

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

            ApplyConfig(typedConfig.AzureConfigPath, typedConfig);

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
            context.Info("Waiting for cloud service deployment status to complete...");
            deployment = client.WaitForCloudServiceDeploymentStatus(typedConfig.ServiceName, typedConfig.DeploymentSlot, DeploymentItem.EnumDeploymentItemStatus.Running, TimeSpan.FromMinutes(typedConfig.AzureTimeoutMinutes));
            context.Info("Waiting for cloud service instance status to complete...");
            deployment = client.WaitForAllCloudServiceInstanceStatus(typedConfig.ServiceName, typedConfig.DeploymentSlot, RoleInstance.EnumInstanceStatus.ReadyRole, TimeSpan.FromMinutes(typedConfig.AzureTimeoutMinutes));
            context.Info("Azure deployment complete");
            //_logger.Info("Done DeployCloudService.InternalExecute");
            //return context.BuildResult();
        }

        private void ApplyConfig(string filePath, DeployCloudServiceTaskConfig config)
        {
            if(config != null && config.RoleList != null && config.RoleList.Any())
            {
                if(!File.Exists(filePath))
                {
                    throw new Exception("Azure Config File not found: " + filePath);
                }
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                string namespaceUri = xmlDoc.DocumentElement.NamespaceURI;
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
                namespaceManager.AddNamespace("azure", namespaceUri);
                bool anyUpdate = false;
                foreach(var key in config.RoleList.Keys)
                {
                    string roleName = key;
                    var role = config.RoleList[key];
                    var roleNode = xmlDoc.SelectSingleNode(string.Format("/azure:ServiceConfiguration/azure:Role[@name='{0}']", roleName), namespaceManager);
                    if(roleNode == null)
                    {
                        throw new Exception("Could not find role " + roleName + " in " + filePath);
                    }
                    if(role.InstanceCount.HasValue)
                    {
                        var instancesNode = roleNode.SelectSingleNode("azure:Instances", namespaceManager);
                        if(instancesNode == null)
                        {
                            instancesNode = xmlDoc.CreateElement("Instances", namespaceUri);
                            roleNode.AppendChild(instancesNode);
                        }
                        var countAttribute = instancesNode.Attributes["count"];
                        if(countAttribute == null)
                        {
                            countAttribute = xmlDoc.CreateAttribute("count", namespaceUri);
                            instancesNode.Attributes.Append(countAttribute);
                        }
                        countAttribute.Value = role.InstanceCount.Value.ToString();
                        anyUpdate = true;
                    }

                    if(role.ConfigurationSettingValues != null && role.ConfigurationSettingValues.Any())
                    {
                        var configurationSettingsNode = roleNode.SelectSingleNode("azure:ConfigurationSettings", namespaceManager);
                        if(configurationSettingsNode == null)
                        {
                            xmlDoc.CreateElement("ConfigurationSettings", namespaceUri);
                            roleNode.AppendChild(configurationSettingsNode);
                        }
                        foreach(var item in role.ConfigurationSettingValues)
                        {
                            var settingNode = configurationSettingsNode.SelectSingleNode(string.Format("azure:Setting[@name='{0}']", item.Key), namespaceManager);
                            if(settingNode == null)
                            {
                                settingNode = xmlDoc.CreateElement("Setting", namespaceUri);
                                configurationSettingsNode.AppendChild(settingNode);

                                var nameAttribute = xmlDoc.CreateAttribute("name");
                                nameAttribute.Value = item.Key;
                                settingNode.Attributes.Append(nameAttribute);
                            }

                            var valueAttribute = settingNode.Attributes["value"];
                            if(valueAttribute == null)
                            {
                                valueAttribute = xmlDoc.CreateAttribute("value");
                                settingNode.Attributes.Append(valueAttribute);
                            }
                            valueAttribute.Value = item.Value;
                        }
                    }
                    if(role.CertificateThumbprints != null && role.CertificateThumbprints.Any())
                    {
                        var certificatesNode = roleNode.SelectSingleNode("azure:Certificates", namespaceManager);
                        if(certificatesNode == null)
                        {
                            xmlDoc.CreateElement("Certificates", namespaceUri);
                            roleNode.AppendChild(certificatesNode);
                        }
                        foreach(var item in role.CertificateThumbprints)
                        {
                            var certificateItemNode = certificatesNode.SelectSingleNode(string.Format("azure:Certificate[@name='{0}']", item.Key), namespaceManager);
                            if(certificateItemNode == null)
                            {
                                certificateItemNode = xmlDoc.CreateElement("Certificate", namespaceUri);
                                certificatesNode.AppendChild(certificateItemNode);

                                var nameAttribute = xmlDoc.CreateAttribute("name");
                                nameAttribute.Value = item.Key;
                                certificateItemNode.Attributes.Append(nameAttribute);
                            }

                            var thumbprintAttribute = certificateItemNode.Attributes["thumbprint"];
                            if(thumbprintAttribute == null)
                            {
                                thumbprintAttribute = xmlDoc.CreateAttribute("thumbprint");
                                certificateItemNode.Attributes.Append(thumbprintAttribute);
                            }
                            thumbprintAttribute.Value = item.Value;
                        }
                    }
                }

                if(anyUpdate)
                {
                    xmlDoc.Save(filePath);
                }
            }
        }
    }
}
