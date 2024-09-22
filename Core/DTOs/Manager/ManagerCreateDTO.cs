using Core.DTOs.Base;

namespace Core.DTOs.Manager;
public class ManagerCreateDTO :BaseCreateDTO
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
