﻿using Core.DTOs.Base;

namespace Core.DTOs.Employee;
public class EmployeeCreateDTO :BaseCreateDTO
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
