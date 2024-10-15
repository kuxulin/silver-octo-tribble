using AutoMapper;
using Core.DTOs.ApplicationImage;
using Core.Entities;

namespace Infrastructure.Mappings;

class ApplicationImageProfile:Profile
{
    public ApplicationImageProfile()
    {
        CreateMap<ApplicationImage, ApplicationImageReadDTO>();
        CreateMap<ApplicationImageCreateDTO, ApplicationImage>();
    }
}

