using FluentAssertions;
using WebsiteLambda.Business.Helpers;
using WebsiteLambda.Business.Interface;
using WebsiteLambda.Models;
using Xunit;

namespace WebsiteLambda.Business.Tests
{
    public class ProjectUpdateHelperTests
    {
        private readonly IProjectUpdateHelper _toTest;
        public ProjectUpdateHelperTests()
        {
            _toTest = new ProjectUpdateHelper();
        }

        [Fact]
        public void GetUpdatedProject_UpdatesOne()
        {
            var older = new Project
            {
                AlternateId = "one"
            };

            var newer = new Project
            {
                AlternateId = "two"
            };

            var result = _toTest.CompareAndGetUpdatedProject(older, newer);

            result.AlternateId.Should().Be("two");
        }
    }
}
