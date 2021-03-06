﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Publisher
{
    public class BuildPublishOptions
    {
        public string Directory { get; set; }
        public string File { get; set; }
        public string ApiUrl { get; set; }
        public string ProjectId { get; set; }
        public string ComponentId { get; set; }
        public string BranchId { get; set; }
        public string Version { get; set; }
        public string NewFileName { get; set; }
        public string FilePattern { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DeployEnvironmentName { get; set; }
    }
}
