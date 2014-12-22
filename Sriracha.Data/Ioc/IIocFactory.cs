using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Ioc
{
    public interface IIocFactory
    {
        T Get<T>(Dictionary<Type, object> parameters = null);
        object Get(Type type, Dictionary<Type, object> parameters = null);

        void RegisterAssembly(Assembly assembly);
    }
}
