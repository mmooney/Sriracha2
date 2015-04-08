using Autofac;
using Sriracha.Data.Deployment;
using Sriracha.Data.Deployment.DeploymentImpl;
using Sriracha.Data.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Ioc
{
    public enum EnumIocMode
    {
        Service,
        DeploymentRunner,
        Web
    }

    public static class SrirachaIocProvider
    {
        public static IIocFactory Initialize(EnumIocMode iocMode)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new SrirachaAutofacModule(iocMode));
            var container = builder.Build();

            //SetupColorConsoleLogging();


            return container.Resolve<IIocFactory>();
        }
    }

}
