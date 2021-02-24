using WebsiteLambda.Models;

namespace WebsiteLambda.Business.Interface
{
    public interface IProjectUpdateHelper
    {
        Project CompareAndGetUpdatedProject(Project newProject, Project oldProject);
    }
}
