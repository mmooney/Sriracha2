using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sriracha.DeployTask.Azure.DeployCloudService
{
    public class DeployCloudServiceTaskConfig
    {  
        public class CloudServiceRole
        {
            public int? InstanceCount { get; set; }
            public Dictionary<string, string> ConfigurationSettingValues { get; set; }
            public Dictionary<string, string> CertificateThumbprints { get; set; }
        }

        [Required]
        public string AzureSubscriptionIdentifier { get; set; }

        [Required]
        public string AzureManagementCertificate { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [DefaultValue("srirachadeploystorage")]
        public string StorageAccountName { get; set; }

        [Required]
        public string AzurePackagePath { get; set; }

        [Required]
        public string AzureConfigPath { get; set; }

        [DefaultValue("production")]
        public string DeploymentSlot { get; set; }

        [DefaultValue(30)]
        public int AzureTimeoutMinutes { get; set; }

        public Dictionary<string,CloudServiceRole> RoleList { get; set; }
    }
}
