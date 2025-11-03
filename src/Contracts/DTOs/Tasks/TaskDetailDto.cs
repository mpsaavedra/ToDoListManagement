using Bootler.Contracts.DTOs.Users;

namespace Bootler.Contracts.DTOs.Tasks;

public record TaskDetailDto
{
    public long Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string StateType { get; init; } = string.Empty;
    public DateTime? DueDate { get; init; }
    public ICollection<UserTaskDto> Users { get; init; } = new List<UserTaskDto>();
}