using AutoMapper;
using Core.DTOs.User;
using Core.Entities;

namespace Infrastructure.Mappings;

class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserReadDTO>()
            .ForMember(dto=> dto.RoleIds,options=>options.MapFrom(u => u.UserRoles.Select(ur => ur.RoleId)));
    }
}
