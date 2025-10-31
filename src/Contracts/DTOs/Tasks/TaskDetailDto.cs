using Bootler.Contracts.DTOs.Users;

namespace Bootler.Contracts.DTOs.Tasks;

public record TaskDetailDto(long Id, string Title, string Description, string StateType, DateTime? DueDate, ICollection<UserDetailDto> Users);
