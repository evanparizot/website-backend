using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Data.Interface
{
    public interface IProjectAccessor
    {
        Task CreateProject(Project project);

        Task<Project> GetProject(Guid id);
        Task<Project> GetProject(string id);
        Task<ProjectDetails> GetProjectDetails(string id);
        Task<ICollection<ProjectDetails>> GetProjectDetails();

        Task UpdateProject(Project project);
        Task UpdateProjectDetails(Guid id, ProjectDetails details);
        Task UpdateProjectDetails(string alternateId, ProjectDetails details);

        Task RemoveProject(Guid id);
        Task RemoveProject(string alternateId);
    }
}
