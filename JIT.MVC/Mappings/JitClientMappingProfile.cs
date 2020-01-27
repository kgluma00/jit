using AutoMapper;
using JIT.Core.DTOs;
using JIT.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JIT.MVC.Mappings
{
    public class JitClientMappingProfile : Profile
    {
        public JitClientMappingProfile()
        {
            CreateMap<UserDto, UserViewModel>();
            CreateMap<UserViewModel, UserDto>().ReverseMap();
            CreateMap<ProjectDto, ProjectViewModel>();
            CreateMap<ProjectViewModel, ProjectDto>().ReverseMap();
        }
    }
}
