using Bootler.Contracts.DTOs.Roles;
using Bootler.Contracts.DTOs.Tasks;

namespace Bootler.Contracts.DTOs.Users;

public record UserDetailDto(long Id, string UserName, RoleDto Role, ICollection<UserTaskDto> Tasks, ICollection<UserTaskDto> AssignedTasks);
