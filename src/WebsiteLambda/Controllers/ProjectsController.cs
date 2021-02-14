using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Website.Logic.Interface;
using Website.Models;

namespace WebsiteLambda.Controllers
{
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private IProjectManager _projectManager;
        private ILogger _logger;

        public ProjectsController(IProjectManager projectManager, ILogger<ProjectsController> logger)
        {
            _projectManager = projectManager;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<Project> GetProject(Guid id)
        {
            _logger.Log(LogLevel.Information, "Hello there");
            return await _projectManager.GetProject(id);
        }
    }
}
