using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using WebsiteLambda.Business.Interface;
using WebsiteLambda.Controllers;
using WebsiteLambda.Mapper;
using Xunit;
using WebsiteLambda.Models.DTO;

namespace WebsiteLambda.Tests
{
    public class ProjectsControllerTests
    {
        private Mock<IProjectManager> _mockProjectManager;
        private IMapper _mapper;
        private Mock<ILogger<ProjectsController>> _mockLogger;

        private readonly ProjectsController _toTest;

        public ProjectsControllerTests()
        {
            _mockProjectManager = new Mock<IProjectManager>();
            var config = new MapperConfiguration(c => c.AddProfile(new AutoMapperProfile()));
            _mapper = config.CreateMapper();
            _mockLogger = new Mock<ILogger<ProjectsController>>();

            _toTest = new ProjectsController(_mockProjectManager.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task TestMapping()
        {
            var input = new CreateProjectRequest
            {
                ContentBody = "Test"
            };

            var result = await _toTest.CreateProjectAsync(input);
        }
    }
}
