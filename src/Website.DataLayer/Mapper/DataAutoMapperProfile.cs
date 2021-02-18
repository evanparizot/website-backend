using Website.Models;

namespace Website.Data.Mapper
{
    public class DataAutoMapperProfile : AutoMapper.Profile
    {
        public DataAutoMapperProfile()
        {
            CreateMap<Project, Models.Project>().ReverseMap();
        }
    }
}
