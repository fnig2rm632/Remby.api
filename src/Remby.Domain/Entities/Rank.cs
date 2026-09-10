using Remby.Domain.Common;

namespace Remby.Domain.Entities;

public sealed class Rank : Entity<int>
{
    public string Name { get; private set; }
    public int TimeRepeat { get; private set; }
    
    private Rank(int id, string name, int timeRepeat) : base(id)
    {
        Name = name;
        TimeRepeat = timeRepeat;
    }

    public static Result<Rank> Create(int id, string name, int timeRepeat)
    {
        var success = ValidateCreationParameters(id, name, timeRepeat);

        if (!success.IsSuccess)
            return success.Error;
        
        var user = new Rank(id, name, timeRepeat);
        
        return user;
    }

    private static Result ValidateCreationParameters(int id, string name, int timeRepeat)
    {
        if (id < 1)
            return Error.Rank.EmptyId;
        
        if (string.IsNullOrWhiteSpace(name))
            return Error.Rank.EmptyName;
        
        if (timeRepeat == 0)
            return Error.Rank.MinDate;
        
        return Result.Success();
    }
}