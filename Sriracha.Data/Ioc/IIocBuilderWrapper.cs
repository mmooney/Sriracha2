using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Ioc
{
    public interface IIocBuilderWrapper
    {
        void Register<InterfaceType, ImplementationType>() where ImplementationType : InterfaceType;
    }
}
