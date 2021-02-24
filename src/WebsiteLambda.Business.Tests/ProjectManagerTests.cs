using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using WebsiteLambda.Business.Interface;
using WebsiteLambda.Data.Interface;
using Xunit;

namespace WebsiteLambda.Business.Tests
{
    public class ProjectManagerTests
    {
        private readonly Mock<IProjectUpdateHelper> _projectUpdateHelper;
        private readonly Mock<IProjectAccessor> _projectAccessor;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ProjectManager>> _logger;

        private readonly IProjectManager _toTest;

        public ProjectManagerTests()
        {
            _projectUpdateHelper = new Mock<IProjectUpdateHelper>();
            _projectAccessor = new Mock<IProjectAccessor>();

            _logger = new Mock<ILogger<ProjectManager>>();

            _toTest = new ProjectManager(_projectUpdateHelper.Object, _projectAccessor.Object, _mapper, _logger.Object);
        }

        [Fact]
        public async Task CreateProject_CreatesProject()
        {

        }

        [Fact]
        public async Task GetProject_ReturnsProject()
        {

        }
    }
}
