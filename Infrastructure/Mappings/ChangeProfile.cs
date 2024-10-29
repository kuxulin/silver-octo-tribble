using AutoMapper;
using Core.DTOs.Change;
using Core.Entities;

namespace Infrastructure.Mappings;

class ChangeProfile : Profile
{
    public ChangeProfile()
    {
        CreateMap<ChangeCreateDTO, Change>();
        CreateMap<Change, ChangeReadDTO>().ForMember(dto => dto.IsRead,
            options => options.MapFrom((c, dto, _, context) => context.TryGetItems(out var items) && items.TryGetValue("userId", out object? value) && c.UserChanges.First(c => c.UserId == (int)value).IsRead));
    }
}
