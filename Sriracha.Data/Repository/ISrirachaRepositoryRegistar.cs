using Sriracha.Data.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Repository
{
    public interface ISrirachaRepositoryRegistar
    {
        void RegisterRepositories(IIocBuilderWrapper builder);
    }
}
