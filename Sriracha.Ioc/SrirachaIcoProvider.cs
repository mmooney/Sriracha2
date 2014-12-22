using SimpleInjector;
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
        DeploymentRunner
    }

    public static class SrirachaIcoProvider
    {
        public static IIocContainer Initialize(EnumIocMode iocMode)
        {
            var container = new Container();

            container.Register<IDeployTaskRunner, DeployTaskRunner>();
            container.Register<IIocContainer>(()=>new SimpleInjectorIocContainer(container));

            return container.GetInstance<IIocContainer>();
        }
    }
}
