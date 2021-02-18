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

        public async Task<Project> CreateProject(Project project)
        {
            var toSave = new Project
            {
                Id = Guid.NewGuid(),
                ProjectDetails = project.ProjectDetails,
                Content = project.Content,
            };

            toSave.ProjectDetails.CreatedDate = DateTime.Now;
            toSave.ProjectDetails.LastUpdatedDate = DateTime.Now;

            return await _projectAccessor.CreateProject(toSave);
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
