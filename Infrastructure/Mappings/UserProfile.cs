using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Infrastructure.Mappings;

class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();
    }
}
