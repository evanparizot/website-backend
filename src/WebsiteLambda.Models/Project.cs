using System;

namespace WebsiteLambda.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string AlternateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string ContentBody { get; set; }
    }
}
