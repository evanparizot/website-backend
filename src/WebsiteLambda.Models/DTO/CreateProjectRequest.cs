using System.ComponentModel.DataAnnotations;

namespace WebsiteLambda.Models.DTO
{
    public class CreateProjectRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ContentBody { get; set; }
    }
}
