using Microsoft.AspNetCore.Mvc;
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

        public ProjectsController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        [HttpGet("{id}")]
        public async Task<Project> GetProject(Guid id)
        {
            return await _projectManager.GetProject(id);
        }
    }
}
