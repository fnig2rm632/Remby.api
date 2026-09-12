namespace Remby.Application.SQRS.Responses.Card;

public class CardResponse
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Hint { get; private set; } = string.Empty;
    public string Decision { get; private set; } = string.Empty;
    public string Folder { get; private set; } = string.Empty;
}