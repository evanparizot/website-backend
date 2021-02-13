using Amazon.DynamoDBv2.DataModel;
using System;

namespace Website.Data.Models
{
    public class Project
    {
        [DynamoDBHashKey("Id")]
        public Guid Id { get; set; }

        [DynamoDBProperty("AlternateId")]
        [DynamoDBGlobalSecondaryIndexHashKey("AlternateId")]
        public string AlternateId { get; set; }

        [DynamoDBProperty("Title")]
        public string Title { get; set; }

        [DynamoDBProperty("Description")]
        public string Description { get; set; }

        [DynamoDBProperty("ThumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [DynamoDBProperty("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [DynamoDBProperty("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [DynamoDBProperty("S3ContentLocation")]
        public string S3ContentLocation { get; set; }
    }
}
