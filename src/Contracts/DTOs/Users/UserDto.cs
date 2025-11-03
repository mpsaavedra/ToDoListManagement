using Bootler.Contracts.DTOs.Roles;
using Bootler.Contracts.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.DTOs.Users;


public record UserDto(long Id, string UserName, RoleSingleDto Role); //, ICollection<UserTaskDto> Tasks, ICollection<UserTaskDto> AssignedTasks);
