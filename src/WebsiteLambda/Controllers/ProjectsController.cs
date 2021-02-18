using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Website.Logic.Interface;
using Website.Models;
using WebsiteLambda.DTO;

namespace WebsiteLambda.Controllers
{
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private IProjectManager _projectManager;
        private IMapper _mapper;
        private ILogger _logger;

        public ProjectsController(IProjectManager projectManager, IMapper mapper, ILogger<ProjectsController> logger)
        {
            _projectManager = projectManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult<Project>> CreateProjectAsync(CreateProjectRequest request)
        {

            var projectToBeSaved = _mapper.Map<Project>(request);

            var project = await _projectManager.CreateProject(projectToBeSaved);

            return project;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project>> GetProjectAsync(Guid id)
        {
            var project = await _projectManager.GetProject(id);

            if (null == project)
            {
                return NotFound();
            }

            return project;
        }
    }
}
