using Core.DTOs.Base;
using Core.DTOs.Project;
using Core.DTOs.User;

namespace Core.DTOs.Manager;
public class ManagerReadDTO :BaseReadDTO
{
    public int? UserId { get; set; }
    public UserReadDTO User { get; set; }
}
