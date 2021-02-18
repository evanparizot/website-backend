using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using AutoMapper;
using Microsoft.Extensions.Logging;
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
        private IDynamoDBContext _context;
        private IAmazonS3 _s3Client;
        private IMapper _mapper;
        private ILogger _logger;
        private readonly AwsResourceConfig _config;
        private const string AlternateIdIndexName = "alternateId";
        private const string VersionPrefix = "v_";
        private const string BaseVersion = "v_0";

        public ProjectAccessor(IDynamoDBContext context, IAmazonS3 s3Client, IMapper mapper, ILogger<ProjectAccessor> logger, IOptions<AwsResourceConfig> options)
        {
            _context = context;
            _s3Client = s3Client;
            _mapper = mapper;
            _logger = logger;
            _config = options.Value;
        }

        public async Task<Project> CreateProject(Project project)
        {
            var toSave = _mapper.Map<Data.Models.Project>(project);

            toSave.LatestVersion = BaseVersion;
            toSave.Version = BaseVersion;

            // Save content to S3 (if this fails then don't save anything to dynamo)
            
            // Save project with version 0; maybe can just use version 0 as the start at all times
            // Use s3 location when saving project


            throw new NotImplementedException();
        }

        public async Task<Project> GetProject(string id)
        {
            //var config = new DynamoDBOperationConfig
            //{
            //    IndexName = AlternateIdIndexName
            //};

            //var project = await _context.LoadAsync<Data.Models.Project>(id, config);

            //return _mapper.Map<Project>(project);
            throw new NotImplementedException();
        }

        public async Task<Project> GetProject(Guid id)
        {
            var project = await _context.LoadAsync<Data.Models.Project>(id, BaseVersion);

            return _mapper.Map<Project>(project);
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

        private string IncrementVersion(string version)
        {
            if (!version.Contains(VersionPrefix))
            {
                throw new ArgumentException();
            }

            if (Int32.TryParse(version.Split("_")[1], out int parsed))
            {
                return "v_" + (parsed + 1);
            } else
            {
                throw new ArgumentException();
            }
        }
    }
}
