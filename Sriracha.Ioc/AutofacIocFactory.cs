using Autofac;
using Sriracha.Data.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sriracha.Ioc
{
    class AutofacIocFactory : IIocFactory
    {
        private IComponentContext _context;

        public AutofacIocFactory(IComponentContext context)
        {
            _context = context;
        }

        public T Get<T>(Dictionary<Type, object> parameters = null)
        {
            if (parameters != null && parameters.Count > 0)
            {
                var parameterArray = parameters.Select(i => new TypedParameter(i.Key, i.Value)).ToArray();
                return _context.Resolve<T>(parameterArray);
            }
            else
            {
                return _context.Resolve<T>();
            }
        }



        public object Get(Type type, Dictionary<Type, object> parameters = null)
        {
            if (parameters != null && parameters.Count > 0)
            {
                var parameterArray = parameters.Select(i => new TypedParameter(i.Key, i.Value)).ToArray();
                return _context.Resolve(type, parameterArray);
            }
            else
            {
                return _context.Resolve(type);
            }
        }

        public void RegisterAssembly(System.Reflection.Assembly assembly)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(assembly).AsSelf();
            builder.Update(_context.ComponentRegistry);
        }
    }
}
