﻿using Core.DTOs.Base;

namespace Core.DTOs.Employee;
public class EmployeeUpdateDTO :BaseUpdateDTO
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
