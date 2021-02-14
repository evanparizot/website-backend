using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Data.Interface;
using Website.Models;
using Website.Models.Configuration;

namespace Website.DataLayer
{
    public class ProjectAccessor : IProjectAccessor
    {
        private IDynamoDBContext _ddbContext;
        private IAmazonS3 _s3Client;
        private readonly AwsResourceConfig _config;

        public ProjectAccessor(IDynamoDBContext context, IAmazonS3 s3Client, IOptions<AwsResourceConfig> options)
        {
            _ddbContext = context;
            _s3Client = s3Client;
            _config = options.Value;
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
            return await Task.FromResult(new Project
            {
                Content = _config.ProjectBucketName
            });
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
