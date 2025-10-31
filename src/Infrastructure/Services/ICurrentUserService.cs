namespace Bootler.Infrastructure.Services;

public interface ICurrentUserService
{
    long? GetUserId();
    string? GetUserName();
    bool? IsAdmin();
    bool IsAuthenticated();
}
