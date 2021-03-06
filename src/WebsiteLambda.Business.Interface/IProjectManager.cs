using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteLambda.Models;

namespace WebsiteLambda.Business.Interface
{
    public interface IProjectManager
    {
        Task<Project> CreateProject(Project project);
        
        Task<Project> GetProject(Guid id);
        Task<IEnumerable<Project>> GetProjects();

    }
}
