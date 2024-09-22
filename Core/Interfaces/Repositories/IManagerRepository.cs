using Core.DTOs.Manager;

namespace Core.Interfaces.Repositories;
public interface IManagerRepository : ICRUDRepository<ManagerReadDTO,ManagerCreateDTO,ManagerUpdateDTO>
{
}
