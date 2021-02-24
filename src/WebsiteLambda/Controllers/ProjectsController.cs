using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebsiteLambda.Business.Interface;
using WebsiteLambda.Models;
using WebsiteLambda.Models.DTO;

namespace WebsiteLambda.Controllers
{
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectManager _projectManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProjectsController(IProjectManager projectManager, IMapper mapper, ILogger<ProjectsController> logger)
        {
            _projectManager = projectManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Project>> CreateProjectAsync([FromBody] CreateProjectRequest request)
        {
            var toSave = _mapper.Map<Project>(request);

            var project = await _projectManager.CreateProject(toSave);

            return project;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project>> GetProjectAsync(Guid id)
        {
            var project = await _projectManager.GetProject(id);

            if (project == default(Project))
            {
                return NotFound();
            }

            return project;
        }
    }
}
