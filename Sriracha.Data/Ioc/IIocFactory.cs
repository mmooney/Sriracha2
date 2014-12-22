using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Ioc
{
    public interface IIocFactory
    {
        T Get<T>(Dictionary<Type, object> parameters = null);
    }
}
