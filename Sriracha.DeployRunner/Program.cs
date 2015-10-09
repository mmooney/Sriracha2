﻿using CommandLine;
using CommandLine.Text;
using Common.Logging;
using MMDB.Shared;
using Sriracha.Data.Deployment;
using Sriracha.Data.Deployment.DeploymentImpl;
using Sriracha.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployRunner
{
    class Program
    {
        public enum EnumOutputFormat
        {
            Text, 
            Json
        }
        public class CommandLineOptions
        {
            [Option('d', "workingDirectory")]
            public string WorkingDirectory { get; set; }

            [Option('l', "logFile")]
            public string LogFile { get; set; }

            [Option('c', "configFile")]
            public string ConfigFile { get; set; }

            [Option('t', "taskBinary", Required=true)]
            public string TaskBinary { get; set; }

            [Option('n', "taskName", Required = true)]
            public string TaskName { get; set; }

            [Option('p', "Pause")]
            public bool Pause { get; set; }

            [Option('o', "OutputFormat")]
            public EnumOutputFormat OutputFormat { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                    (HelpText current) =>
                        HelpText.DefaultParsingErrorsHandler(this, current));
            }

        }
        
        static int Main(string[] args)
        {
            var iocContainer = SrirachaIocProvider.Initialize(EnumIocMode.DeploymentRunner);
            var logger = iocContainer.Get<ILog>();
            bool pause = false;
            int result = 0;

            try 
            {
                var options = new CommandLineOptions();
                if (!Parser.Default.ParseArguments(args, options))
                {
                    throw new Exception(options.GetUsage());
                }
                pause = options.Pause;

                //string workingDirectory = StringHelper.IsNullOrEmpty(options.WorkingDirectory, Path.GetDirectoryName(Path.GetFullPath(options.TaskBinary)));
                string workingDirectory = StringHelper.IsNullOrEmpty(options.WorkingDirectory, Environment.CurrentDirectory);

                var taskRunner = iocContainer.Get<IDeployTaskRunner>();
                switch(options.OutputFormat)
                {
                    case EnumOutputFormat.Text:
                        var textStatusReporter = iocContainer.Get<LoggerDeployStatusReporter>();
                        try 
                        {
                            taskRunner.RunTask(iocContainer.Get<LoggerDeployStatusReporter>(), options.TaskBinary, options.TaskName, options.ConfigFile, workingDirectory);
                        }
                        catch(Exception err)
                        {
                            result = 1;
                            textStatusReporter.ErrorException(err);
                        }
                        break;
                    case EnumOutputFormat.Json:
                        using (var outputStream = Console.OpenStandardOutput())
                        using (var jsonStatusReporter = new JsonDeployStatusReporter(outputStream))
                        {
                            try 
                            {
                                taskRunner.RunTask(jsonStatusReporter, options.TaskBinary, options.TaskName, options.ConfigFile, workingDirectory);
                            }
                            catch(Exception err)
                            {
                                result = 1;
                                jsonStatusReporter.ErrorException(err);
                            }
                        }
                        break;
                }
            }
            catch(Exception err)
            {
                logger.Error(err);
                result = 1;
            }

            if(pause)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            return result;
        }
    }
}
