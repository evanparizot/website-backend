using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Data.Interface;
using Website.Logic.Interface;
using Website.Models;

namespace Website.Logic
{
    public class ProjectManager : IProjectManager
    {
        private IProjectAccessor _projectAccessor;

        public ProjectManager(IProjectAccessor projectAccessor)
        {
            _projectAccessor = projectAccessor;
        }

        public async Task CreateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetProject(Guid id)
        {
            return await _projectAccessor.GetProject(id);
        }

        public Task<ProjectDetails> GetProjectDetails(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProjectDetails>> GetProjectDetails()
        {
            throw new NotImplementedException();
        }
    }
}
