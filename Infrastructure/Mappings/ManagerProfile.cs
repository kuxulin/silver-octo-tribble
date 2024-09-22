using AutoMapper;
using Core.DTOs.Manager;
using Core.Entities;

namespace Infrastructure.Mappings;
internal class ManagerProfile :Profile
{
    public ManagerProfile()
    {
        CreateMap<ManagerCreateDTO, Manager>();
        CreateMap<ManagerUpdateDTO, Manager>();
        CreateMap<Manager, ManagerReadDTO>();
    }
}
