﻿using Core.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Employee;
public class EmployeeCreateDTO :BaseCreateDTO
{
    public string FullName { get; set; }
    [RegularExpression(@"^\d{10,13}$", ErrorMessage = "Entered phone format is not valid.")]
    public string PhoneNumber { get; set; }
}
