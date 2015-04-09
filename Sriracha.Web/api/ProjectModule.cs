using Nancy;
using Nancy.ModelBinding;
using Sriracha.Data.Dto.Projects;
using Sriracha.Data.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sriracha.Web.api
{
    public class ProjectModule : NancyModule
    {
        private readonly IProjectManager _projectManager;

        public ProjectModule(IProjectManager projectManager) : base("api/project")
        {
            _projectManager = projectManager;

            Get["/"] = _ => "this is a project list";
            Post["/"] = _ => 
            {
                var project = this.Bind<Project>();
                return _projectManager.CreateProject(project.ProjectName);
            };
        }
    }
}