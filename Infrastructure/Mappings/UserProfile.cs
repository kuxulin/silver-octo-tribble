using AutoMapper;
using Core.DTOs.Auth;
using Core.DTOs.User;
using Core.Entities;

namespace Infrastructure.Mappings;

class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserReadDTO>()
            .ForMember(dto => dto.RoleIds, options => options.MapFrom(u => u.UserRoles.Select(ur => ur.RoleId)))
            .ForMember(dto => dto.ManagerId, options => options.MapFrom(u => u.ManagerId))
            .ForMember(dto => dto.EmployeeId, options => options.MapFrom(u => u.EmployeeId));

        CreateMap<RegisterDTO, User>()
           .ForMember(u => u.Image, options => options.Ignore());
    }
}
