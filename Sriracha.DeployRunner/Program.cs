using CommandLine;
using CommandLine.Text;
using Common.Logging;
using Sriracha.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployRunner
{
    class Program
    {
        public class CommandLineOptions
        {
            [Option('d', "workingDirectory")]
            public string WorkingDirectory { get; set; }

            [Option('l', "logFile")]
            public string LogFile { get; set; }

            [Option('c', "configFile", Required = true)]
            public string ConfigFile { get; set; }

            [Option('t', "taskBinary", Required=true)]
            public string TaskBinary { get; set; }

            [Option('n', "taskName", Required = true)]
            public bool TaskName { get; set; }


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
        
        static void Main(string[] args)
        {
            var iocContainer = SrirachaIocProvider.Initialize(EnumIocMode.DeploymentRunner);
            var logger = iocContainer.Get<ILog>();
            logger.Info("hello");
            logger.Error("ERROR");

            Console.ReadKey();
        }
    }
}
