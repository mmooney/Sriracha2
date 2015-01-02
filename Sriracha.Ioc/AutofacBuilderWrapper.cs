using Autofac;
using Sriracha.Data.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sriracha.Ioc
{
    public class AutofacBuilderWrapper : IIocBuilderWrapper
    {
        private readonly ContainerBuilder _builder;

        public AutofacBuilderWrapper(Autofac.ContainerBuilder builder)
        {
            _builder = builder;
        }

        public void Register<InterfaceType, ImplementationType>() where ImplementationType : InterfaceType
        {
            _builder.RegisterType<ImplementationType>().As<InterfaceType>();
        }
    }
}
