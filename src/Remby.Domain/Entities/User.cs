using Remby.Domain.Common;

namespace Remby.Domain.Entities;

public sealed class User : Entity<Guid>
{
    public string Login { get; private set; }
    public DateTime? LastVisit { get; private set; }

    private User(Guid id, string login, DateTime lastVisit) : base(id)
    {
        Login = login;
        LastVisit = lastVisit;
    }
    
    private User(string login, DateTime lastVisit) : base(Guid.Empty)
    {
        Login = login;
        LastVisit = lastVisit;
    }
    
    public static Result<User> Create(Guid id, string login, DateTime lastVisit)
    {
        var success = ValidateCreationParameters(login, lastVisit);

        if (!success.IsSuccess)
            return success.Error;
        
        var user = new User(id, login, lastVisit);
        
        return user;
    }
    
    public static Result<User> Create(string login, DateTime lastVisit)
    {
        var success = ValidateCreationParameters(login, lastVisit);

        if (!success.IsSuccess)
            return success.Error;
        
        var user = new User(login, lastVisit);
        
        return user;
    }

    private static Result ValidateCreationParameters(string login, DateTime lastVisit)
    {
        if(string.IsNullOrWhiteSpace(login))
            return Error.User.EmptyLogin;
        
        if(lastVisit > DateTime.UtcNow)
            return Error.User.DateVisitInFuture;
        
        return Result.Success();
    }

    public Result UpdateLastVisit(DateTime newLastVisit)
    {
        if(newLastVisit > DateTime.Now)
            return Error.User.DateVisitInFuture;
        
        LastVisit = newLastVisit;
        
        return Result.Success();
    }
    
}