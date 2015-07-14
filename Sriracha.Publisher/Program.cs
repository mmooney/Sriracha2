using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Common.Logging;

namespace Sriracha.Publisher
{
    public class Program
    {
        static int Main(string[] args)
        {
            var log = SetupColorConsoleLogging();

            try 
            {
                var options = new Options();
                string action = string.Empty;
                dynamic invokedVerbInstance = new object();
                bool result = CommandLine.Parser.Default.ParseArguments(args, options,
                    (verb, subOptions) =>
                    {
                        action = verb;
                        invokedVerbInstance = subOptions;
                    });
                if (!result)
                {
                    // Values are available here
                    throw new ArgumentException("Error " + CommandLine.Text.HelpText.AutoBuild(options));
                }
                else if(string.IsNullOrEmpty(action))
                {
                    throw new ArgumentException("Missing action parameter");
                }
                else 
                {
                    switch(action.ToLower())
                    {
                        case "publish":
                            var publishOptons = (Options.PublishOptions)invokedVerbInstance;
                            DoPublish(log, publishOptons);
                            break;
                        default:
                            throw new ArgumentException("Unknown action parameter: " + action);
                    }
                    log.Info("good");
                }
                //if (options.Pause)
                //{
                    Console.ReadKey();
                //}
                return 0;
            }
            catch(Exception err)
            {
                log.Fatal(err.ToString());
                return -1;
            }


        }

        private static void DoPublish(ILog log, Options.PublishOptions options)
        {
            throw new NotImplementedException();
#if false
            VerifyParameter(options.ApiUrl, "Publish", "ApiUrl", "apiurl", 'a');
            VerifyParameter(options.ProjectId, "Publish", "ProjectId", "project", 'p');
            //VerifyParameter(options.BranchId,"Publish", "BranchId", "branch");
            VerifyParameter(options.ComponentId, "Publish", "ComponentId", "component", 'c');
            VerifyParameter(options.Version, "Publish", "Version", "version", 'v');
            if ((!string.IsNullOrEmpty(options.UserName) && string.IsNullOrEmpty(options.Password))
                || (string.IsNullOrEmpty(options.UserName) && !string.IsNullOrEmpty(options.Password)))
            {
                throw new Exception("UserName (--userName) and Password (--password) must be used together for Publish");
            }
            if (!string.IsNullOrWhiteSpace(options.File))
            {
                if (!string.IsNullOrWhiteSpace(options.Directory))
                {
                    throw new Exception("File (--file|-f) and Directory (--directory|-f) cannot be both be used together for Publish");
                }
                if (!string.IsNullOrWhiteSpace(options.FilePattern))
                {
                    throw new Exception("File (--file|-f) and FilePattern (--filePattern) cannot be both be used together for Publish");
                }
                PublishFile(options.File, options.ApiUrl, options.ProjectId, options.ComponentId, options.BranchId, options.Version, options.NewFileName, options.UserName, options.Password, options.DeployNow);
            }
            else if (!string.IsNullOrWhiteSpace(options.Directory))
            {
                if (!string.IsNullOrWhiteSpace(options.FilePattern))
                {
                    throw new Exception("Directory (--directory|-d) and FilePattern (--filePattern) cannot be both be used together for Publish");
                }
                PublishDirectory(options.Directory, options.ApiUrl, options.ProjectId, options.ComponentId, options.BranchId, options.Version, options.UserName, options.Password, options.DeployNow);
            }
            else if (!string.IsNullOrWhiteSpace(options.FilePattern))
            {
                PublishFilePattern(options.FilePattern, options.ApiUrl, options.ProjectId, options.ComponentId, options.BranchId, options.Version, options.NewFileName, options.UserName, options.Password, options.DeployNow);
            }
            else
            {
                throw new Exception("Either File (--file|-f), Directory (--directory|-f), or FilePattern (--filePattern) required for Publish");
            }
            break;
#endif
        }

        private static void VerifyParameter(string value, string actionName, string parameterName, string longName, char? shortName = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                string message;
                if (shortName.HasValue)
                {
                    message = string.Format("{0} (--{1}|-{2}) required for {3}", parameterName, longName, shortName, actionName);
                }
                else
                {
                    message = string.Format("{0} (--{1}) required for {2}", parameterName, longName, actionName);
                }
                throw new ArgumentException(message);
            }
        }

        private static void PublishFile(ILog logger, string filePath, string apiUrl, string projectId, string componentId, string branchId, string version, string newFileName, string userName, string password, string deployEnvironmentName)
        {
            var publisher = new BuildPublisher(logger);
            var options = new BuildPublishOptions
            {
                File = filePath,
                ApiUrl = apiUrl,
                ProjectId = projectId,
                ComponentId = componentId,
                BranchId = branchId,
                Version = version,
                NewFileName = newFileName,
                UserName = userName,
                Password = password,
                DeployEnvironmentName = deployEnvironmentName
            };
            publisher.PublishFile(options);
        }

        private static void PublishFilePattern(ILog logger, string filePattern, string apiUrl, string projectId, string componentId, string branchId, string version, string newFileName, string userName, string password, string deployEnvironmentName)
        {
            var publisher = new BuildPublisher(logger);
            var options = new BuildPublishOptions
            {
                FilePattern = filePattern,
                ApiUrl = apiUrl,
                ProjectId = projectId,
                ComponentId = componentId,
                BranchId = branchId,
                Version = version,
                NewFileName = newFileName,
                UserName = userName,
                Password = password,
                DeployEnvironmentName = deployEnvironmentName
            };
            publisher.PublishFilePattern(options);
        }

        private static void PublishDirectory(ILog logger, string directoryPath, string apiUrl, string projectId, string componentId, string branchId, string version, string userName, string password, string deployEnvironmentName)
        {
            var publisher = new BuildPublisher(logger);
            var options = new BuildPublishOptions
            {
                Directory = directoryPath,
                ApiUrl = apiUrl,
                ProjectId = projectId,
                ComponentId = componentId,
                BranchId = branchId,
                Version = version,
                UserName = userName,
                Password = password,
                DeployEnvironmentName = deployEnvironmentName
            };
            publisher.PublishDirectory(options);
        }

        private static ILog SetupColorConsoleLogging()
        {
            Common.Logging.LogManager.Adapter = new Common.Logging.NLog.NLogLoggerFactoryAdapter((Common.Logging.Configuration.NameValueCollection)null);

            var loggingConfig = NLog.LogManager.Configuration;
            if (loggingConfig == null)
            {
                loggingConfig = new NLog.Config.LoggingConfiguration();
            }
            var consoleTarget = new NLog.Targets.ColoredConsoleTarget();
            consoleTarget.Layout = "${longdate}:${message} ${exception:format=message,stacktrace=\r\n}";
            loggingConfig.AddTarget("consoleTarget", consoleTarget);
            var rule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Trace, consoleTarget);
            loggingConfig.LoggingRules.Add(rule);
            NLog.LogManager.Configuration = loggingConfig;


            return Common.Logging.LogManager.GetLogger(typeof(Program));
        }
    }
}
