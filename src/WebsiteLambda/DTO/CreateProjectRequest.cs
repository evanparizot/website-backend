namespace WebsiteLambda.DTO
{
    public class CreateProjectRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Content { get; set; }
    }
}
