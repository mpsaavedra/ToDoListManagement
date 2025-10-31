namespace Bootler.Contracts.Requests.Tasks;

public record TaskAssignToUser(long userId, long taskId, long adminId);
