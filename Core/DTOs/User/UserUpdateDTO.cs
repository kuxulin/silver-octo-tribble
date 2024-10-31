using Core.DTOs.ApplicationImage;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.User;
public class UserUpdateDTO
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [RegularExpression(@"^\+\d{8,15}$", ErrorMessage = "Entered phone format is not valid.")]
    public string? PhoneNumber { get; set; }
    public ApplicationImageCreateDTO? ImageDto {  get; set; } 
    public string AccessToken { get; set; } = null!;
}
