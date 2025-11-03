namespace Bootler.Contracts.Requests.Tasks;

public record TaskAssignToUserRequest(long UserId, long TaskId, long? AdminId);
