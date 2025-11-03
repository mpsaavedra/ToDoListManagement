using Bootler.Contracts.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.DTOs.Roles;

public record RoleSingleDto(long Id, string Name);
public record RoleDto(long Id, string Name, ICollection<UserDto> Users);
