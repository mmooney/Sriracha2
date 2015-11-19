using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WindowsService.RemoveWindowsService
{
	public class RemoveWindowsServiceTaskConfig
	{
		[DefaultValue("localhost")] public string TargetMachineName { get; set; }
		[Required] public string ServiceName { get; set; }
		public string TargetServiceDirectory { get; set; }
	}
}
