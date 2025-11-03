using Bootler.Contracts.DTOs.Tasks;

namespace Bootler.Contracts.DTOs.Users;

public record UserTaskDto(long Id, UserDto User, TaskDto Task, UserDto? AssignedBy);
