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
using dropkick.Configuration.Dsl.WinService;
using dropkick.Configuration.Dsl.Security;
using MMDB.Shared;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using dropkick.Wmi;

namespace Sriracha.DeployTask.WindowsService.Dropkick
{
    internal class DropkickWindowsServiceDeployment : Deployment<DropkickWindowsServiceDeployment, DropkickWindowsServiceDeploymentSettings>
    {
		public static Role WindowsService { get; set; }
		public static Role RemoveWindowsService { get; set; }

		public DropkickWindowsServiceDeployment()
        {
            Define(settings =>
            {
                DeploymentStepsFor(WindowsService,
                                    s =>
                                    {
										if(settings.ExecutingRole != settings.Role)
										{
											return;
										}
                                        if (string.IsNullOrEmpty(settings.SourceServiceDirectory))
                                        {
                                            throw new Exception("Missing SourceServiceDirectory");
                                        }
                                        string serviceExeName;
                                        if (!string.IsNullOrEmpty(settings.ServiceExeName))
                                        {
                                            serviceExeName = settings.ServiceExeName;
                                        }
                                        else
                                        {
                                            var fileList = Directory.GetFiles(settings.SourceServiceDirectory, "*.exe");
                                            if (fileList == null || !fileList.Any())
                                            {
                                                throw new Exception("No *.exe files found at: " + settings.SourceServiceDirectory);
                                            }
                                            fileList = fileList.Where(i=>!i.EndsWith(".vshost.exe", StringComparison.CurrentCultureIgnoreCase)).ToArray();
                                            if (fileList.Length > 1)
                                            {
                                                throw new Exception("Multiple *.exe files found at: " + settings.SourceServiceDirectory + ", Files: " + string.Join(",", fileList.Select(i => Path.GetFileName(i))));
                                            }
											serviceExeName = Path.GetFileName(fileList.First());
                                        }
                                        if (!string.IsNullOrEmpty(settings.TargetMachineUserName) && !string.IsNullOrEmpty(settings.TargetMachinePassword))
                                        {
                                            s.WithAuthentication(settings.TargetMachineUserName, settings.TargetMachinePassword);

                                            s.OpenFolderShareWithAuthentication(@"{{TargetServiceDirectory}}", settings.TargetMachineUserName, settings.TargetMachinePassword);
                                        }

                                        ApplySettings(s, settings, serviceExeName);

                                        var serviceName = "{{ServiceName}}";
                                        s.WinService(serviceName).Stop();

                                        var copy = s.CopyDirectory(settings.SourceServiceDirectory).To(@"{{TargetServiceDirectory}}");
                                        if (settings.DeleteTargetBeforeDeploy)
                                        {
                                            copy.DeleteDestinationBeforeDeploying();
                                        }

                                        if (!string.IsNullOrEmpty(settings.SourceExeConfigPath))
                                        {
                                            s.CopyFile(settings.SourceExeConfigPath).ToDirectory(@"{{TargetServiceDirectory}}");
                                        }

                                        s.Security(o =>
                                        {
                                            o.LocalPolicy(lp =>
                                            {
                                                lp.LogOnAsService(settings.ServiceUserName);
                                                lp.LogOnAsBatch(settings.ServiceUserName);
                                            });

                                            o.ForPath(settings.TargetServiceDirectory, fs => fs.GrantRead(settings.ServiceUserName));
                                            //    //o.ForPath(Path.Combine(settings.TargetServiceDirectory,"logs"), fs => fs.GrantReadWrite(settings.ServiceUserName));
                                        });
                                        s.WinService(serviceName).Delete();
                                        var service = s.WinService(serviceName).Create()
                                                .WithCredentials(settings.ServiceUserName, StringHelper.IsNull(settings.ServiceUserPassword,string.Empty))
                                                .WithDisplayName(settings.ServiceName)
                                                .WithServicePath(Path.Combine(settings.TargetServiceDirectory, serviceExeName))
                                                .WithStartMode(settings.ServiceStartMode);
                                        if (!string.IsNullOrEmpty(settings.ServiceDependencies))
                                        {
                                            var list = settings.ServiceDependencies.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var item in list)
                                            {
                                                service = service.AddDependency(item);
                                            }
                                        }

                                        if (settings.ServiceStartMode != ServiceStartMode.Disabled && settings.ServiceStartMode != ServiceStartMode.Manual)
                                        {
                                            s.WinService(serviceName).Start();
                                        }
                                    });

				DeploymentStepsFor(RemoveWindowsService,
									s =>
									{
										if (settings.ExecutingRole != settings.Role)
										{
											return;
										}
										if (string.IsNullOrEmpty(settings.ServiceName))
										{
											throw new Exception("Missing ServiceName");
										}
										s.WinService(settings.ServiceName).Stop();
										s.WinService(settings.ServiceName).Delete();
									});
			});

        }

        private void ApplySettings(ProtoServer s, DropkickWindowsServiceDeploymentSettings settings, string serviceExeName)
        {
            if(!string.IsNullOrEmpty(settings.AppSettingValuesJson) || !string.IsNullOrEmpty(settings.ConnectionStringValuesJson) || !string.IsNullOrEmpty(settings.XpathValuesJson))
            {
                string exeConfigPath;
                if (!string.IsNullOrEmpty(settings.SourceExeConfigPath))
                {
                    exeConfigPath = settings.SourceExeConfigPath;
                }
                else 
                {
                    if(string.IsNullOrEmpty(settings.SourceServiceDirectory))
                    {
                        throw new Exception("Missing SourceExeConfigPath and SourceServiceDirectory");
                    }
                    if(!string.IsNullOrEmpty(serviceExeName))
                    {
                        exeConfigPath = Path.Combine(settings.SourceServiceDirectory, serviceExeName + ".config");
                    }
                    else 
                    {
                        var fileList = Directory.GetFiles(settings.SourceServiceDirectory, "*.exe.config");
                        if(fileList == null || !fileList.Any())
                        {
                            throw new Exception("No *.exe.config files found at: " + settings.SourceServiceDirectory);
                        }
                        if(fileList.Length > 1)
                        {
                            throw new Exception("Multiple *.exe.config files found at: " + settings.SourceServiceDirectory + ", Files: " + string.Join(",", fileList.Select(i=>Path.GetFileName(i))));
                        }
                        exeConfigPath = fileList.First();
                    }
                }
                if (!File.Exists(exeConfigPath))
                {
                    return;
                }

                bool anyUpdate = false;
                //string tempWebConfig = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "web.config");
                //File.Copy(sourceWebConfig, tempWebConfig);
                string tempWebConfig = exeConfigPath;
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load(tempWebConfig);
                if (!string.IsNullOrEmpty(settings.AppSettingValuesJson))
                {
                    var appSettingValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.AppSettingValuesJson);
                    foreach (var dataItem in appSettingValues)
                    {
                        string key = dataItem.Key;
                        string valueData = dataItem.Value;

                        var configurationNode = xmlDoc.SelectSingleNode("configuration");
                        if (configurationNode == null)
                        {
                            throw new Exception("configuration node not found");
                        }
                        var appSettingsNode = configurationNode.SelectSingleNode("appSettings");
                        if (appSettingsNode == null)
                        {
                            appSettingsNode = xmlDoc.CreateElement("appSettings");
                            configurationNode.AppendChild(appSettingsNode);
                        }
                        var itemNode = appSettingsNode.SelectSingleNode(string.Format("add[@key='{0}']", key.Replace("'", "''")));
                        if (itemNode == null)
                        {
                            itemNode = xmlDoc.CreateElement("add");
                            appSettingsNode.AppendChild(itemNode);

                            var keyAttribute = xmlDoc.CreateAttribute("key");
                            keyAttribute.Value = key;
                            itemNode.Attributes.Append(keyAttribute);
                        }
                        var valueAttribute = itemNode.Attributes["value"];
                        if (valueAttribute == null)
                        {
                            valueAttribute = xmlDoc.CreateAttribute("value");
                            itemNode.Attributes.Append(valueAttribute);
                        }
                        valueAttribute.Value = valueData;
                        anyUpdate = true;
                    }
                }

                if (!string.IsNullOrEmpty(settings.ConnectionStringValuesJson))
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

                if (!string.IsNullOrEmpty(settings.XpathValuesJson))
                {
                    var xPathValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.XpathValuesJson);
                    foreach (var dataItem in xPathValues)
                    {
                        string xpath = dataItem.Key;
                        string valueData = dataItem.Value;

                        var nodeList = xmlDoc.SelectNodes(xpath);
                        if (nodeList == null || nodeList.Count == 0)
                        {
                            throw new Exception("XPath not found: " + xpath);
                        }
                        foreach (XmlNode node in nodeList)
                        {
                            node.Value = valueData;
                        }
                        anyUpdate = true;

                    }
                }
                if (anyUpdate)
                {
                    xmlDoc.Save(tempWebConfig);
                    //s.CopyFile(tempWebConfig).ToDirectory(@"{{TargetWebsitePath}}\web.config");
                }
                //File.Delete(tempWebConfig);
            }
        }

        public Dictionary<string, string> GetDefaultServerMap()
        {
            var returnValue = new Dictionary<string, string>();
			returnValue.Add("WindowsService", string.Empty);
			returnValue.Add("RemoveWindowsService", string.Empty);
			return returnValue;
        }
    }
}
