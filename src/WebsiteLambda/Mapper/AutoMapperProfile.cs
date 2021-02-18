﻿using Website.Models;
using WebsiteLambda.DTO;

namespace WebsiteLambda.Mapper
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<CreateProjectRequest, ProjectDetails>();
            CreateMap<CreateProjectRequest, Project>()
                .ForMember(d => d.ProjectDetails, opt => opt.MapFrom(s => s));

        }
    }
}
