using AutoMapper;
using Core.DTOs.ApplicationImage;
using Core.Entities;

namespace Infrastructure.Mappings;

class ApplicationImageProfile:Profile
{
    public ApplicationImageProfile()
    {
        CreateMap<ApplicationImage, ImageReadDTO>();
        CreateMap<ImageCreateDTO, ApplicationImage>()
            .ForMember(i => i.Content, options => options.MapFrom(dto => Convert.FromBase64String(dto.Content)));
    }
}