using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteLambda.Models;

namespace WebsiteLambda.Data.Interface
{
    public interface IProjectAccessor
    {
        Task<Project> CreateProject(Project project);

        Task<Project> GetProject(Guid id);

        /// <summary>
        /// Returns all available projects.
        /// </summary>
        /// <param name="withContent">Used to indicate retrieval of project content. Default is false, with content being null.</param>
        /// <returns></returns>
        Task<IEnumerable<Project>> GetProjects(int pageSize = 10, bool withContent = false);

        //Task UpdateProject(Project project);
        //Task UpdateProjectDetails(Guid id, ProjectDetails details);
        //Task UpdateProjectDetails(string alternateId, ProjectDetails details);

        //Task RemoveProject(Guid id);
        //Task RemoveProject(string alternateId);
    }
}
