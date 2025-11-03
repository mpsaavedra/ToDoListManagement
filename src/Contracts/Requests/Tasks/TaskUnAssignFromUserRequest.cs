namespace Bootler.Contracts.Requests.Tasks;

public record TaskUnAssignFromUserRequest(long UserId, long TaskId, long? AdminId);
