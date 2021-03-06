using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebsiteLambda.Data.Interface;
using WebsiteLambda.Models;
using WebsiteLambda.Models.Configuration;

namespace WebsiteLambda.Data
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
            var toSave = _mapper.Map<Models.Project>(project, opt =>
            {
                opt.Items["Version"] = BaseVersion;
                opt.Items["LatestVersion"] = BaseVersion;
            });

            var request = new PutObjectRequest
            {
                BucketName = _config.ProjectsBucketName,
                Key = GetS3CompositeKey(toSave.Id, BaseVersion),
                ContentBody = project.ContentBody
            };

            await _s3Client.PutObjectAsync(request);
            await _context.SaveAsync(toSave);

            return project;
        }

        public async Task<Project> GetProject(Guid id)
        {
            var project = await _context.LoadAsync<Models.Project>(id, BaseVersion);

            if (project == null)
            {
                return default;
            }

            var contentBody = await GetS3ContentBody(GetS3CompositeKey(id, project.LatestVersion));
            return _mapper.Map<Project>(project, opts => opts.Items["ContentBody"] = contentBody);
        }

        ///<inheritdoc cref="IProjectAccessor"/>
        public async Task<IEnumerable<Project>> GetProjects(bool withContent = false)
        {
            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition(nameof(Models.Project.Version), ScanOperator.Equal, BaseVersion)
            };

            var search =  _context.ScanAsync<Models.Project>(scanConditions);
            var projects = new List<Models.Project>();
            do
            {
                projects.AddRange(await search.GetNextSetAsync());
            }
            while (!search.IsDone);

            return _mapper.Map<IEnumerable<Project>>(projects);
        }

        #region Helper Methods
        private string IncrementVersion(string version)
        {
            if (!version.Contains(VersionPrefix))
            {
                throw new ArgumentException();
            }

            if (int.TryParse(version.Split("_")[1], out int parsed))
            {
                return "v_" + (parsed + 1);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private string GetS3CompositeKey(Guid id, string version)
        {
            return id.ToString() + "." + version;
        }

        private async Task<string> GetS3ContentBody(string key)
        {
            var contentBody = "";

            try
            {
                using (GetObjectResponse response = await _s3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = _config.ProjectsBucketName,
                    Key = key
                }))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    contentBody = reader.ReadToEnd();
                }
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError("Unable to retrieve specified S3 content", e);
            }
           
            return contentBody;
        }
        #endregion

    }
}
