﻿using Core.DTOs.ApplicationImage;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Auth;

public class RegisterDTO
{
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
    [RegularExpression(@"^\+\d{8,15}$", ErrorMessage = "Entered phone format is not valid.")]
    public string PhoneNumber { get; set; } = null!;
    public ImageCreateDTO Image { get; set; } = null!;
}

