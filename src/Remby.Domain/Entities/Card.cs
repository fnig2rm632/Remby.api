using Remby.Domain.Common;

namespace Remby.Domain.Entities;

public sealed class Card : Entity<int>
{
    public string Title { get; private set; }
    public string Hint { get; private set; }
    public string Decision { get; private set; }
    public int FolderId { get; private set; }
    public DateTime LastRepeat { get; private set; }
    public DateTime DeleteAt { get; private set; }
    
    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly List<User> _users = new();
    public IReadOnlyCollection<User> Users => _users.AsReadOnly();
    
    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly List<Folder> _folders = new();
    public IReadOnlyCollection<Folder> Folders => _folders.AsReadOnly();

    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly List<Rank> _ranks = new();
    public IReadOnlyCollection<Rank> Ranks => _ranks.AsReadOnly();
    
    private Card(int id, string title, string hint, string decision) : base(id)
    {
        Title = title;
        Hint = hint;
        Decision = decision;
    }
    
    private Card(int id, string title, string hint, string decision, int folderId) : base(id)
    {
        Title = title;
        Hint = hint;
        Decision = decision;
        FolderId = folderId;
    }
    
    private Card(string title, string hint, string decision) : base(0)
    {
        Title = title;
        Hint = hint;
        Decision = decision;
    }
    
    private Card(string title, string hint, string decision, int folderId) : base(0)
    {
        Title = title;
        Hint = hint;
        Decision = decision;
        FolderId = folderId;
    }

    public static Result<Card> Create(int id, string title, string hint, string decision, int folderId = -1)
    {
        var success = ValidateCreationParameters(title, hint, decision);

        if (success.IsSuccess)
            return success.Error;
        
        var card = folderId == -1 
            ? new Card(id, title, hint, decision) 
            : new Card(id, title, hint, decision, folderId);

        return card;
    }
    
    public static Result<Card> Create(string title, string hint, string decision, int folderId = -1)
    {
        var success = ValidateCreationParameters(title, hint, decision);

        if (success.IsSuccess)
            return success.Error;
        
        var card = folderId == -1 
            ? new Card( title, hint, decision) 
            : new Card( title, hint, decision, folderId);

        return card;
    }

    private static Result ValidateCreationParameters(string title, string hint, string decision)
    {
        if (string.IsNullOrEmpty(title))
            return Error.Card.EmptyTitle;

        if (title.Length > 50)
            return Error.Card.TitleTooLong(50);
        
        if (hint.Length > 50)
            return Error.Card.HintTooLong(50);
        
        if (string.IsNullOrEmpty(decision))
            return Error.Card.EmptyDecision;
        
        if (decision.Length > 255)
            return Error.Card.DecisionTooLong(255);
        
        return Result.Success();
    }

    public Result Delete(DateTime deleteAt)
    {
        if (deleteAt > DateTime.UtcNow)
            return Error.Card.DeleteAtInFuture;
        
        DeleteAt = deleteAt;
        
        return Result.Success();
    }
    
    public Result UpdateLastRepeat(DateTime lastRepeat)
    {
        if (lastRepeat > DateTime.UtcNow)
            return Error.Card.DeleteAtInFuture;
        
        LastRepeat = lastRepeat;
        
        return Result.Success();
    }
}