using AutoMapper;
using Core.Entities;
using Infrastructure.ViewModels;

namespace Infrastructure.Mappings;
internal class ProjectProfile :Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectCreateViewModel, Project>();
    }
}
