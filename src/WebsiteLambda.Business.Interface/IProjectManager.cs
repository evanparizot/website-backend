using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteLambda.Models;

namespace WebsiteLambda.Business.Interface
{
    public interface IProjectManager
    {
        Task<Project> CreateProjectAsync(Project project);
        
        Task<Project> GetProjectAsync(Guid id);
        Task<IEnumerable<Project>> GetProjectsAsync();

    }
}
