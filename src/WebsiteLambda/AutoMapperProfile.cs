using WebsiteLambda.Models;
using WebsiteLambda.Models.DTO;

namespace WebsiteLambda.Mapper
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProjectRequest, Project>();

            CreateMap<Project, Project>();
        }
    }
}
