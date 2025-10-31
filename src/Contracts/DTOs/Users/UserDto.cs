using Bootler.Contracts.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.DTOs.Users;


public record UserDto(long Id, string UserName, string Role, ICollection<TaskDto> Tasks, ICollection<TaskDto> AssignedTasks);
