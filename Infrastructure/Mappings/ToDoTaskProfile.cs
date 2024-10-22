using AutoMapper;
using Core.DTOs.Manager;
using Core.DTOs.TodoTask;
using Core.Entities;
using Core.Enums;

namespace Infrastructure.Mappings
{
    internal class ToDoTaskProfile :Profile
    {
        public ToDoTaskProfile()
        {
            CreateMap<TodoTaskCreateDTO, TodoTask>()
                .ForMember(t => t.StatusId,options => options.MapFrom(dto => AvailableTaskStatus.Todo));

            CreateMap<TodoTaskUpdateDTO, TodoTask>();
            CreateMap<TodoTask, TodoTaskReadDTO>()
                .ForMember(dto => dto.Status, options => options.MapFrom(t => t.StatusId));
        }
    }
}
