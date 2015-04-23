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
            this.PublishVerb = new PublishSubOptions();
        }

        [VerbOption("publish")]
        public PublishSubOptions PublishVerb { get; set; }

        public class PublishSubOptions
        {
            [Option('x', "url")]
            public string Url { get; set; }

            [Option('u', "user")]
            public string UserName { get; set; }

            [Option('u', "password")]
            public string Password { get; set; }
        }
    }
}
