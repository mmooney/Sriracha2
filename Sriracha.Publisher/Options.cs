using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Publisher
{
    public class Options
    {
        public Options()
        {
            this.PublishVerb = new PublishOptions();

        }

        [VerbOption("publish")]
        public PublishOptions PublishVerb { get; set; }

        [Option('p', "pause")]
        public bool Pause { get; set; }

        public class PublishOptions
        {
            [Option('u', "apiurl")]
            public string ApiUrl { get; set; }

            [Option('d', "directory")]
            public string Directory { get; set; }

            [Option('f', "file")]
            public string File { get; set; }

            [Option("userName")]
            public string UserName { get; set; }

            [Option("password")]
            public string Password { get; set; }

            [Option("filePattern")]
            public string FilePattern { get; set; }

            [Option("newfilename")]
            public string NewFileName { get; set; }

            [Option('p', "project")]
            public string ProjectId { get; set; }

            [Option('c', "component")]
            public string ComponentId { get; set; }

            [Option('v', "version")]
            public string Version { get; set; }

            public string BranchId { get; set; }
        }
    }
}
