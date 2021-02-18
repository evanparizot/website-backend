using Amazon.DynamoDBv2.DataModel;
using System;

namespace Website.Data.Models
{
    [DynamoDBTable("projects")]
    public class Project
    {
        [DynamoDBHashKey("id")]
        public Guid Id { get; set; }

        [DynamoDBRangeKey("version")]
        public string Version { get; set; }

        [DynamoDBProperty("latestVersion")]
        public string LatestVersion { get; set; }

        [DynamoDBProperty("AlternateId")]
        [DynamoDBGlobalSecondaryIndexHashKey("AlternateId")]
        public string AlternateId { get; set; }

        [DynamoDBProperty("title")]
        public string Title { get; set; }

        [DynamoDBProperty("description")]
        public string Description { get; set; }

        [DynamoDBProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [DynamoDBProperty("s3ContentLocation")]
        public string S3ContentLocation { get; set; }

        [DynamoDBProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [DynamoDBProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }
}
