using Core.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Manager;
public class ManagerCreateDTO :BaseCreateDTO
{
    public string FullName { get; set; }
    [RegularExpression(@"^\d{10,13}$", ErrorMessage = "Entered phone format is not valid.")]
    public string PhoneNumber { get; set; }
}
