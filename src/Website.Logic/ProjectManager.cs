using Microsoft.Extensions.Logging;
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
        private ILogger _logger;

        public ProjectManager(IProjectAccessor projectAccessor, ILogger<ProjectManager> logger)
        {
            _projectAccessor = projectAccessor;
            _logger = logger;
        }

        public async Task CreateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetProject(Guid id)
        {
            _logger.LogInformation("I'm in the project manager. " + id);
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
