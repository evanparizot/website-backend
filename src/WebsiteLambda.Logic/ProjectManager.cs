using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteLambda.Data.Interface;
using WebsiteLambda.Logic.Interface;
using WebsiteLambda.Models;

namespace WebsiteLambda.Logic
{
    public class ProjectManager : IProjectManager
    {
        private IProjectAccessor _projectAccessor;
        private IMapper _mapper;
        private ILogger _logger;

        public ProjectManager(IProjectAccessor projectAccessor, IMapper mapper, ILogger<ProjectManager> logger)
        {
            _projectAccessor = projectAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Project> CreateProject(Project project)
        {
            var toSave = _mapper.Map<Project>(project);

            toSave.Id = Guid.NewGuid();
            toSave.ProjectDetails.CreatedDate = toSave.ProjectDetails.LastUpdatedDate = DateTime.Now;

            _logger.LogInformation("Saving new project with id: {id}", toSave.Id);

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
