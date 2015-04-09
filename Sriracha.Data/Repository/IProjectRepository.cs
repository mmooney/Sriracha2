using Sriracha.Data.Dto.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Repository
{
    public interface IProjectRepository
    {
        Project Create(string projectName);
        List<Project> GetList();
        Project Get(Guid id);
    }
}
