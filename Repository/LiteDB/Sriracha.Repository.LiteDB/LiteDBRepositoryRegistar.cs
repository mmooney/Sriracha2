using Sriracha.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Repository.LiteDB
{
    public class LiteDBRepositoryRegistar : ISrirachaRepositoryRegistar
    {
        public void RegisterRepositories(Data.Ioc.IIocBuilderWrapper builder)
        {
            builder.Register<IProjectRepository, LiteDBProjectRepository>();
            builder.Register<IUserRepository, LiteDBUserRepository>();
        }
    }
}
