using Core.DTOs.Base;

namespace Core.DTOs.Manager;
public class ManagerUpdateDTO :BaseUpdateDTO
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
