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

    }
}
