using AutoMapper;
using Core.DTOs.Change;
using Core.Entities;

namespace Infrastructure.Mappings;

class ChangeProfile : Profile
{
    public ChangeProfile()
    {
        CreateMap<ChangeCreateDTO, Change>();
        CreateMap<Change, ChangeReadDTO>();
    }
}
