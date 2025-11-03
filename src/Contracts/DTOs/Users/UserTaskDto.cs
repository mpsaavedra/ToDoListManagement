using Bootler.Contracts.DTOs.Tasks;

namespace Bootler.Contracts.DTOs.Users;

public record UserTaskDto
{
    public long Id { get; set; }
    public UserDto User { get; set; }
    public TaskDto Task { get; set; }
    public UserDto? AssignedBy { get; set; }
}
