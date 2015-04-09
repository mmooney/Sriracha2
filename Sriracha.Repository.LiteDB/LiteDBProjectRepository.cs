using Sriracha.Data.Dto.Projects;
using Sriracha.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Sriracha.Data.Identity;
using MMDB.Shared;

namespace Sriracha.Repository.LiteDB
{
    public class LiteDBProjectRepository : BaseLiteDBRepository<Project>, IProjectRepository
    {
        private readonly ISrirachaIdentity _identity;

        public LiteDBProjectRepository(ISrirachaIdentity identity)
        {
            _identity = identity;
        }

        protected override LiteCollection<Project> EnsureIndexes(LiteCollection<Project> collection)
        {
            collection.EnsureIndex(x=>x.ProjectName, true);
            return collection;
        }

        public Project Create(string projectName)
        {
            if(string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentNullException("projectName");
            }
            using(var db = this.GetDB())
            {
                var collection = this.GetCollection(db);
                if(collection.Exists(x=>x.ProjectName == projectName))
                {
                    throw new ArgumentException("Duplicate projectName: " + projectName);
                }
                var project = new Project
                {
                    Id = Guid.NewGuid(),
                    ProjectName = projectName
                };
                project.SetCreatedFields(_identity.UserName);
                collection.Insert(project);
                db.Commit();
                return project;
            }
        }

        public List<Project> GetList()
        {
            using(var db = this.GetDB())
            {
                var collection = this.GetCollection(db);
                return collection.FindAll().ToList();
            }
        }


        public Project Get(Guid id)
        {
            using (var db = this.GetDB())
            {
                var collection = this.GetCollection(db);
                var item = collection.FindById(id);
                if(item == null)
                {
                    throw new RecordNotFoundException<Project>(id);
                }
                return item;
            }
        }
    }
}
