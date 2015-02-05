using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dropkick.Configuration.Dsl;
using dropkick.Configuration.Dsl.Files;
using dropkick.Configuration.Dsl.Authentication;
using dropkick.Configuration.Dsl.Iis;
using MMDB.Shared;


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

                                        string appPoolName = settings.ApplicationPoolName;
                                        if (string.IsNullOrWhiteSpace(appPoolName))
                                        {
                                            appPoolName = settings.SiteName;
                                        }
                                        var iis = s.Iis7Site(settings.SiteName, @"{{TargetWebsitePath}}", default(int))
                                            .VirtualDirectory(StringHelper.IsNullOrEmpty(settings.VirtualDirectoryName, "/"))
                                            .SetAppPoolTo(appPoolName, pool =>
                                            {
                                                pool.SetRuntimeToV4();
                                                //pool.UseClassicPipeline();
                                                //pool.Enable32BitAppOnWin64();
                                            }).SetPathTo(@"{{TargetWebsitePath}}");
                                    });
            });
        }

        public Dictionary<string, string> GetDefaultServerMap()
        {
            var returnValue = new Dictionary<string, string>();
            returnValue.Add("Website", string.Empty);
            return returnValue;
        }
    }
}
