using Sriracha.Data.Ioc;
using Sriracha.DeployTask.WebApplication.Dropkick;
using Sriracha.DeployTask.WebApplication.Dropkick.DropkickImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.WebApplication
{
    public class DeployWebApplicationSrirachaIocRegistar : ISrirachaIocRegistar
    {
        public void RegisterTypes(IIocBuilderWrapper builderWrapper)
        {
            builderWrapper.Register<IDropkickRunner, DropkickRunner>();
        }
    }
}
