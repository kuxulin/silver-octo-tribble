using AutoMapper;
using Core.DTOs.Manager;
using Core.DTOs.TodoTask;
using Core.Entities;

namespace Infrastructure.Mappings
{
    internal class ToDoTaskProfile :Profile
    {
        public ToDoTaskProfile()
        {
            CreateMap<TodoTaskCreateDTO, TodoTask>();
            CreateMap<TodoTaskUpdateDTO, TodoTask>();
            CreateMap<TodoTask, TodoTaskReadDTO>();
        }
    }
}
