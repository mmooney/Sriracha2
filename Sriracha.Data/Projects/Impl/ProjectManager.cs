using Sriracha.Data.Dto.Projects;
using Sriracha.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Projects.Impl
{
    public class ProjectManager : IProjectManager
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectManager(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Project CreateProject(string projectName)
        {
            return _projectRepository.Create(projectName);
        }

        public List<Project> GetProjectList()
        {
            return _projectRepository.GetList();
        }


        public Project GetProject(Guid id)
        {
            return _projectRepository.Get(id);
        }
    }
}
