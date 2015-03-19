using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dropkick.Configuration.Dsl;
using dropkick.Configuration.Dsl.Files;
using dropkick.Configuration.Dsl.Authentication;
using dropkick.Configuration.Dsl.Iis;
using dropkick.Configuration.Dsl.Xml;
using MMDB.Shared;
using System.IO;
using Newtonsoft.Json;
using System.Xml;


namespace Sriracha.DeployTask.WebApplication.Dropkick
{
    internal class DropkickWebDeployment : Deployment<DropkickWebDeployment, DropkickWebDeploymentSettings>
    {
        public static Role Website { get; set; }

        public DropkickWebDeployment()
        {
            Define(settings =>
            {
                DeploymentStepsFor(Website,
                                    s =>
                                    {
                                        if (!string.IsNullOrEmpty(settings.TargetMachineUserName) && !string.IsNullOrEmpty(settings.TargetMachinePassword))
                                        {
                                            s.WithAuthentication(settings.TargetMachineUserName, settings.TargetMachinePassword);

                                            s.OpenFolderShareWithAuthentication(@"{{TargetWebsitePath}}", settings.TargetMachineUserName, settings.TargetMachinePassword);
                                        }

                                        //Do NOT delete destination before deploying.  You will delete the Invoices and Assets folders, which we need to keep.
                                        var x =  s.CopyDirectory(settings.SourceWebsitePath).To(@"{{TargetWebsitePath}}");
                                        if(settings.DeleteTargetBeforeDeploy)
                                        {
                                            x.DeleteDestinationBeforeDeploying();
                                        }

                                        this.ApplySettings(s, settings);

                                        string appPoolName = settings.ApplicationPoolName;
                                        if (string.IsNullOrWhiteSpace(appPoolName))
                                        {
                                            appPoolName = settings.SiteName;
                                        }
                                        string virtualDirectory = StringHelper.IsNull(settings.VirtualDirectoryName, string.Empty);
                                        if(virtualDirectory == "/")
                                        {
                                            virtualDirectory = string.Empty;
                                        }
                                        var iis = s.Iis7Site(settings.SiteName, @"{{TargetWebsitePath}}", default(int))
                                            .VirtualDirectory(virtualDirectory)
                                            .SetAppPoolTo(appPoolName, pool =>
                                            {
                                                pool.SetRuntimeToV4();
                                                //pool.UseClassicPipeline();
                                                //pool.Enable32BitAppOnWin64();
                                            }).SetPathTo(@"{{TargetWebsitePath}}");
                                    });
            });
        }

        private void ApplySettings(ProtoServer s, DropkickWebDeploymentSettings settings)
        {
            if(string.IsNullOrEmpty(settings.SourceWebsitePath))
            {
                throw new Exception("Missing SourceWebsitePath");
            }
            string sourceWebConfig = Path.Combine(settings.SourceWebsitePath, "web.config");
            if(!File.Exists(sourceWebConfig))
            {
                return;
            }

            bool anyUpdate = false;
            //string tempWebConfig = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "web.config");
            //File.Copy(sourceWebConfig, tempWebConfig);
            string tempWebConfig = sourceWebConfig;
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(tempWebConfig);
            if(!string.IsNullOrEmpty(settings.AppSettingValuesJson))
            {
                var appSettingValues = JsonConvert.DeserializeObject<Dictionary<string,string>>(settings.AppSettingValuesJson);
                foreach (var dataItem in appSettingValues)
                {
                    string key = dataItem.Key;
                    string valueData = dataItem.Value;

                    var configurationNode = xmlDoc.SelectSingleNode("configuration");
                    if(configurationNode == null)
                    {
                        throw new Exception("configuration node not found");
                    }
                    var appSettingsNode = configurationNode.SelectSingleNode("appSettings");
                    if(appSettingsNode == null)
                    {
                        appSettingsNode = xmlDoc.CreateElement("appSettings");
                        configurationNode.AppendChild(appSettingsNode);
                    }
                    var itemNode = appSettingsNode.SelectSingleNode(string.Format("add[@key='{0}']", key.Replace("'","''")));
                    if(itemNode == null)
                    {
                        itemNode = xmlDoc.CreateElement("add");
                        appSettingsNode.AppendChild(itemNode);

                        var keyAttribute = xmlDoc.CreateAttribute("key");
                        keyAttribute.Value = key;
                        itemNode.Attributes.Append(keyAttribute);
                    }
                    var valueAttribute = itemNode.Attributes["value"];
                    if(valueAttribute == null)
                    {
                        valueAttribute = xmlDoc.CreateAttribute("value");
                        itemNode.Attributes.Append(valueAttribute);
                    }
                    valueAttribute.Value = valueData;
                    anyUpdate = true;
                }
            }

            if(!string.IsNullOrEmpty(settings.ConnectionStringValuesJson))
            {
                var connectionStringValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.ConnectionStringValuesJson);
                foreach (var dataItem in connectionStringValues)
                {
                    string key = dataItem.Key;
                    string valueData = dataItem.Value;

                    var configurationNode = xmlDoc.SelectSingleNode("configuration");
                    if (configurationNode == null)
                    {
                        throw new Exception("configuration node not found");
                    }
                    var connectionStringsNode = configurationNode.SelectSingleNode("connectionStrings");
                    if (connectionStringsNode == null)
                    {
                        connectionStringsNode = xmlDoc.CreateElement("connectionStrings");
                        configurationNode.AppendChild(connectionStringsNode);
                    }
                    var itemNode = connectionStringsNode.SelectSingleNode(string.Format("add[@name='{0}']", key.Replace("'", "''")));
                    if (itemNode == null)
                    {
                        itemNode = xmlDoc.CreateElement("add");
                        connectionStringsNode.AppendChild(itemNode);

                        var nameAttribute = xmlDoc.CreateAttribute("name");
                        nameAttribute.Value = key;
                        itemNode.Attributes.Append(nameAttribute);
                    }
                    var connectionStringAttribute = itemNode.Attributes["connectionString"];
                    if (connectionStringAttribute == null)
                    {
                        connectionStringAttribute = xmlDoc.CreateAttribute("connectionString");
                        itemNode.Attributes.Append(connectionStringAttribute);
                    }
                    connectionStringAttribute.Value = valueData;
                    anyUpdate = true;
                }
            }

            if(!string.IsNullOrEmpty(settings.XpathValuesJson))
            {
                var xPathValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.XpathValuesJson);
                foreach (var dataItem in xPathValues)
                {
                    string xpath = dataItem.Key;
                    string valueData = dataItem.Value;

                    var nodeList = xmlDoc.SelectNodes(xpath);
                    if(nodeList == null || nodeList.Count == 0)
                    {
                        throw new Exception("XPath not found: " + xpath);
                    }
                    foreach(XmlNode node in nodeList)
                    {
                        node.Value = valueData;
                    }
                    anyUpdate = true;

                }
            }
            if(anyUpdate)
            {
                xmlDoc.Save(tempWebConfig);
                //s.CopyFile(tempWebConfig).ToDirectory(@"{{TargetWebsitePath}}\web.config");
            }
            //File.Delete(tempWebConfig);
        }

        public Dictionary<string, string> GetDefaultServerMap()
        {
            var returnValue = new Dictionary<string, string>();
            returnValue.Add("Website", string.Empty);
            return returnValue;
        }
    }
}
