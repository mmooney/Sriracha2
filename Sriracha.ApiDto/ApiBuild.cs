using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.ApiDto
{
    public class ApiBuild
    {
        public string FileId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectBranchId { get; set; }
        public string ProjectComponentId { get; set; }
        public string Version { get; set; }
    }
}
