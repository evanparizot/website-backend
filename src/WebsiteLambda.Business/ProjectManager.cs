using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteLambda.Data.Interface;
using WebsiteLambda.Models;
using WebsiteLambda.Business.Interface;

namespace WebsiteLambda.Business
{
    public class ProjectManager : IProjectManager
    {
        private IProjectUpdateHelper _projectUpdateHelper;
        private IProjectAccessor _projectAccessor;
        private IMapper _mapper;
        private ILogger _logger;

        public ProjectManager(IProjectUpdateHelper projectUpdateHelper, IProjectAccessor projectAccessor, IMapper mapper, ILogger<ProjectManager> logger)
        {
            _projectUpdateHelper = projectUpdateHelper;
            _projectAccessor = projectAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            var toSave = _mapper.Map<Project>(project);

            toSave.Id = Guid.NewGuid();
            toSave.CreatedDate = DateTime.Now;
            toSave.LastUpdatedDate = DateTime.Now;

            _logger.LogInformation("Saving new project with id: {id}", toSave.Id);

            return await _projectAccessor.CreateProject(toSave);
        }

        public async Task<Project> GetProjectAsync(Guid id)
        {
            return await _projectAccessor.GetProject(id);
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _projectAccessor.GetProjects();
        }
    }
}
