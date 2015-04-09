using Sriracha.Data.Dto.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Projects
{
    public interface IProjectManager
    {
        Project CreateProject(string projectName);
    }
}
