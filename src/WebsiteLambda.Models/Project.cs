using System;

namespace WebsiteLambda.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string AlternateId { get; set; }
        public ProjectDetails ProjectDetails { get; set; }
        public string ContentBody { get; set; }
    }
}
