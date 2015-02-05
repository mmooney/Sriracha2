using dropkick.Configuration;
using Sriracha.Data.Deployment;
using Sriracha.Data.Impersonation;
using Sriracha.Data.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WebApplication.Dropkick
{
    public class DropkickRunnerContext : IDisposable
    {
        private IProcessRunner _processRunner;
        private TaskExecutionContext _taskExecutionContext;
        private string _dropkickDirectory;
        private IImpersonator _impersonator;

        public DropkickRunnerContext(IProcessRunner processRunner, TaskExecutionContext taskExecutionContext, string dropkickDirectory)
        {
            _processRunner = processRunner;
            _taskExecutionContext = taskExecutionContext;
            _dropkickDirectory = dropkickDirectory;
        }

        public void Dispose()
        {
            //if(!this._disposed)
            //{
            //    if(Directory.Exists(_dropkickDirectory))
            //    {
            //        try 
            //        {
            //            Directory.Delete(_dropkickDirectory);
            //        }
            //    }
            //}
        }

        public void Run<DeploymentType>(DropkickConfiguration settings, Dictionary<string, string> serverMaps, IEnumerable<string> rolesToRun)
        {
            string settingsDirectory = Path.Combine(_dropkickDirectory, "settings");
            if (!Directory.Exists(settingsDirectory))
            {
                Directory.CreateDirectory(settingsDirectory);
            }
            string environmentName = settings.Environment;
            string settingsFilePath = Path.Combine(settingsDirectory, environmentName + ".js");
            File.WriteAllText(settingsFilePath, settings.ToJson());

            string serverMapFilePath = Path.Combine(settingsDirectory, environmentName + ".servermaps");
            File.WriteAllText(serverMapFilePath, serverMaps.ToJson());


            string deploymentFilePath = typeof(DeploymentType).Assembly.Location;

            string roleParameter = string.Join(",", rolesToRun);

            string dropkickExePath = string.Format("{0}\\dk.exe", _dropkickDirectory);
#warning Need to wrap these parameters in quotes, but need a fix in dropkick first.
            string exeParameters = string.Format("execute /deployment:{0} /environment:{1} /settings:{2} /roles:{3} --silent", deploymentFilePath, environmentName, settingsDirectory, roleParameter);

            using (var standardOutputWriter = new StringWriter())
            using (var errorOutputWriter = new StringWriter())
            {
                int exeResult;
                if (string.IsNullOrEmpty(_taskExecutionContext.DeployCredentialsUserName) || string.IsNullOrEmpty(_taskExecutionContext.DeployCredentialsUserPassword))
                {
                    exeResult = _processRunner.Run(dropkickExePath, exeParameters, standardOutputWriter, errorOutputWriter, Path.GetDirectoryName(deploymentFilePath));
                }
                else
                {
                    using (var impersonation = _impersonator.BeginImpersonation(_taskExecutionContext.DeployCredentialsUserName, _taskExecutionContext.DeployCredentialsUserPassword, _taskExecutionContext.DeployCredentialsDomain))
                    {
                        _taskExecutionContext.Info("Starting process as {0} impersonating {1}", WindowsIdentity.GetCurrent().Name, _taskExecutionContext.DeployCredentialsUserName);
                        string fullExePath = Path.GetFullPath(dropkickExePath);
                        _taskExecutionContext.Info("For Options.ExecutablePath {0}, using {1}", dropkickExePath, fullExePath);
                        //result = _processRunner.RunAsUser(exePath, formattedArgs, standardOutputWriter, errorOutputWriter, credentials.Domain, credentials.UserName, password);
                        exeResult = _processRunner.RunAsToken(fullExePath, exeParameters, standardOutputWriter, errorOutputWriter, impersonation.TokenHandle);
                    }
                }
                string standardOutput = standardOutputWriter.GetStringBuilder().ToString();
                string errorOutput = errorOutputWriter.GetStringBuilder().ToString();
                if (exeResult == 0)
                {
                    if (!string.IsNullOrWhiteSpace(standardOutput))
                    {
                        _taskExecutionContext.Info(standardOutput);
                    }
                    if (!string.IsNullOrWhiteSpace(errorOutput))
                    {
                        _taskExecutionContext.Error(errorOutput);
                        throw new Exception("DropKickRunner Failed: " + errorOutput);
                    }
                }
                else
                {
                    string errorText;
                    if (!string.IsNullOrWhiteSpace(errorOutput))
                    {
                        errorText = errorOutput;
                    }
                    else if (!string.IsNullOrWhiteSpace(standardOutput))
                    {
                        errorText = standardOutput;
                    }
                    else
                    {
                        errorText = "Error Code " + exeResult.ToString();
                    }
                    _taskExecutionContext.Error(errorText);
                    throw new Exception("DropkickRunner Task Failed: " + errorText);
                }
            }

        }
    }
}
