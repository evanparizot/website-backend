using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Logic.Interface
{
    public interface IProjectManager
    {
        Task<Project> CreateProject(Project project);

        Task<Project> GetProject(Guid id);
        Task<ProjectDetails> GetProjectDetails(string id);
        Task<ICollection<ProjectDetails>> GetProjectDetails();
    }
}
