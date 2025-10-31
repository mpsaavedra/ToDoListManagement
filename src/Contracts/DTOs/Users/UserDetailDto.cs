using Bootler.Contracts.DTOs.Tasks;

namespace Bootler.Contracts.DTOs.Users;

public record UserDetailDto(long Id, string UserName, string Role, ICollection<TaskDetailDto> Tasks, ICollection<TaskDetailDto> AssignedTasks);
