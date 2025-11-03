using Bootler.Contracts.DTOs.Roles;
using Bootler.Contracts.DTOs.Tasks;

namespace Bootler.Contracts.DTOs.Users;

public record UserDetailDto
{
    public long Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public RoleDto Role { get; init; } = null!;
    public ICollection<UserTaskDto> Tasks { get; init; } = new List<UserTaskDto>();
    public ICollection<UserTaskDto> AssignedTasks { get; init; } = new List<UserTaskDto>();
}