using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Base;
public class BaseReadDTO
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
}
