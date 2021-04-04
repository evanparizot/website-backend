using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using WebsiteLambda.Business.Interface;
using WebsiteLambda.Controllers;
using WebsiteLambda.Mapper;
using Xunit;
using WebsiteLambda.Models.DTO;
using WebsiteLambda.Models;
using FluentAssertions;
using System;
using Microsoft.AspNetCore.Mvc;

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
        public async Task CreateProjectAsync()
        {
            var request = new CreateProjectRequest
            {
                Title = "Title"
            };
            var responseProject = new Project
            {
                Id = Guid.NewGuid(),
                AlternateId = "This is an alternate id"
            };
            _mockProjectManager.Setup(x => x.CreateProjectAsync(It.IsAny<Project>()).Result).Returns(responseProject);

            var response = await _toTest.CreateProjectAsync(request);

            response.Value.Should().Be(responseProject);
        }

        [Fact]
        public async Task GetProjectAsync_SuccessfullyReturnsProject()
        {
            var expectedProject = new Project
            {
                Id = Guid.NewGuid()
            };
            _mockProjectManager.Setup(x => x.GetProjectAsync(It.IsAny<Guid>()).Result).Returns(expectedProject);

            var response = await _toTest.GetProjectAsync(Guid.NewGuid());

            response.Value.Should().Be(expectedProject);
        }

        [Fact]
        public async Task GetProjectAsync_Returns404ForNullProject()
        {
            _mockProjectManager.Setup(x => x.GetProjectAsync(It.IsAny<Guid>())).ReturnsAsync(default(Project));

            var response = await _toTest.GetProjectAsync(Guid.NewGuid());

            response.Result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async Task GetProjectsAsync()
        {

        }
    }
}
