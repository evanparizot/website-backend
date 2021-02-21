using WebsiteLambda.Models;

namespace WebsiteLambda.Data.Mapper
{
    public class DataAutoMapperProfile : AutoMapper.Profile
    {
        public DataAutoMapperProfile()
        {
            CreateMap<Project, Models.Project>()
                .ForMember(d => d.LatestVersion, opt => opt.MapFrom((s, d, _, c) => c.Options.Items["LatestVersion"]))
                .ForMember(d => d.Version, opt => opt.MapFrom((s, d, _, c) => c.Options.Items["Version"]))
                .IncludeMembers(s => s.ProjectDetails);
            CreateMap<ProjectDetails, Models.Project>();

            CreateMap<Models.Project, ProjectDetails>();
            CreateMap<Models.Project, Project>()
                .ForMember(d => d.ContentBody, opt => opt.MapFrom((s, d, _, c) => c.Options.Items["ContentBody"]))
                .ForMember(d => d.ProjectDetails, opt => opt.MapFrom(s => s));
        }
    }
}
