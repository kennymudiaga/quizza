using AutoMapper;
using Quizza.Users.Application.Commands;
using Quizza.Users.Domain.Models;
using Quizza.Users.Domain.Models.Entities;

namespace Quizza.Users.Application.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<SignUpCommand, SignUpRequest>();
        CreateMap<UserProfile, LoginResponse>()
            .ForMember(x => x.Roles, cfg => cfg.MapFrom(src => src.Roles.Select(x => x.Role).ToList()));
    }
}
