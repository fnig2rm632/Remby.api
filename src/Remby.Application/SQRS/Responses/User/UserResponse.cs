namespace Remby.Application.SQRS.Responses.User;

public class UserResponse
{
    public Guid Id { get; private set; }
    public string Login { get; private set; } = string.Empty;
}