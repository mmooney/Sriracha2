using SimpleInjector;
using Sriracha.Data.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sriracha.Ioc
{
    public class SimpleInjectorIocContainer : IIocContainer
    {
        private Container _container;

        public SimpleInjectorIocContainer(Container container)
        {
            // TODO: Complete member initialization
            _container = container;
        }
    }
}
