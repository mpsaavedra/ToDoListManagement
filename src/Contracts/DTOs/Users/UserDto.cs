using Bootler.Contracts.DTOs.Roles;
using Bootler.Contracts.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.DTOs.Users;


public record UserDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public RoleSingleDto Role { get; set; }
    public ICollection<UserTaskDto> Tasks { get; set; }
    public ICollection<UserTaskDto> AssignedTasks { get; set; }
}
