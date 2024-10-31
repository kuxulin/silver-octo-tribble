using AutoMapper;
using Core.DTOs.Project;
using Core.Entities;

namespace Infrastructure.Mappings;
internal class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectCreateDTO, Project>()
            .ForMember(p => p.Managers, options => options.MapFrom(dto => dto.ManagerIds.Select(id => new Manager() { Id = id })))
            .ForMember(p => p.Employees, options => options.MapFrom(dto => dto.EmployeeIds!.Select(id => new Employee() { Id = id })));
        CreateMap<ProjectUpdateDTO, Project>();
        CreateMap<Project, ProjectReadDTO>();
    }
}
