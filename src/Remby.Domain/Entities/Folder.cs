using Remby.Domain.Common;

namespace Remby.Domain.Entities;

public sealed class Folder : Entity<int>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime DeleteAt { get; private set; }
    
    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly List<User> _users = new();
    public IReadOnlyCollection<User> Users => _users.AsReadOnly();
    
    private Folder(int id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
        
    }
    
    private Folder(string name, string description) : base(0)
    {
        Name = name;
        Description = description;
    }

    public static Result<Folder> Create(int id, string name, string description)
    {
        var success = ValidateCreationParameters(name, description);

        if (!success.IsSuccess)
            return success.Error;
        
        var user = new Folder(id ,name, description);
        
        return user;
    }
    
    public static Result<Folder> Create(string name, string description)
    {
        var success = ValidateCreationParameters(name, description);

        if (!success.IsSuccess)
            return success.Error;
        
        var user = new Folder(name, description);
        
        return user;
    }

    private static Result ValidateCreationParameters(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Folder.EmptyName;

        if (name.Length > 50)
            return Error.Folder.NameTooLong(50);
        
        if (description.Length > 255)
            return Error.Folder.DescriptionTooLong(255);
        
        return Result.Success();
    }

    public Result Delete(DateTime deleteAt)
    {
        if (deleteAt > DateTime.UtcNow)
            return Error.Folder.DataInFuture;
        
        DeleteAt = deleteAt;
        
        return Result.Success();
    }
    
}