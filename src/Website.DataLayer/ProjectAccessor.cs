using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Data.Interface;
using Website.Models;

namespace Website.DataLayer
{
    public class ProjectAccessor : IProjectAccessor
    {
        private IDynamoDBContext _ddbContext;
        private IAmazonS3 _s3Client;

        public ProjectAccessor(IDynamoDBContext context, IAmazonS3 s3Client)
        {
            _ddbContext = context;
            _s3Client = s3Client;
        }

        public async Task CreateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetProject(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetProject(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectDetails> GetProjectDetails(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ProjectDetails>> GetProjectDetails()
        {
            throw new NotImplementedException();
        }

        public async Task RemoveProject(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveProject(string alternateId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProjectDetails(Guid id, ProjectDetails details)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProjectDetails(string alternateId, ProjectDetails details)
        {
            throw new NotImplementedException();
        }
    }
}
