using AutoMapper;
using Core.Entities;
using Core.DTOs.Project;
using Core.DTOs.Employee;

namespace Infrastructure.Mappings;
internal class ProjectProfile :Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectCreateDTO, Project>();
        CreateMap<ProjectUpdateDTO, Project>();
        CreateMap<Project, ProjectReadDTO>();
    }
}
