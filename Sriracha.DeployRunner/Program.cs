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
        static void Main(string[] args)
        {
            var iocContainer = SrirachaIcoProvider.Initialize(EnumIocMode.DeploymentRunner);
        }
    }
}
