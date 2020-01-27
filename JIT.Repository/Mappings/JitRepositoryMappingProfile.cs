using AutoMapper;
using JIT.Core.DTOs;
using JIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIT.Repository.Mappings
{
   public class JitRepositoryMappingProfile : Profile
    {
        public JitRepositoryMappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
        }
    }
}
