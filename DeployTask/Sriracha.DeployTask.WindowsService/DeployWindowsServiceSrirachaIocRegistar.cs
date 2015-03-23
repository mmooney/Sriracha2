using Sriracha.Data.Ioc;
using Sriracha.DeployTask.Helper.Dropkick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WindowsService
{
    public class DeployWindowsServiceSrirachaIocRegistar : ISrirachaIocRegistar
    {
        public void RegisterTypes(IIocBuilderWrapper builderWrapper)
        {
            new DropkickSrirachaIocRegistar().RegisterTypes(builderWrapper);
        }
    }
}
