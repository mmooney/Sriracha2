using Sriracha.Data.Ioc;
using Sriracha.DeployTask.Helper.Dropkick.DropkickImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.DeployTask.Helper.Dropkick
{
    public class DropkickSrirachaIocRegistar : ISrirachaIocRegistar
    {
        public void RegisterTypes(IIocBuilderWrapper builderWrapper)
        {
            builderWrapper.Register<IDropkickRunner, DropkickRunner>();
        }
    }
}
