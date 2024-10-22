using Core.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Manager;
public class ManagerCreateDTO :BaseCreateDTO
{
    public int UserId { get; set; }
}
