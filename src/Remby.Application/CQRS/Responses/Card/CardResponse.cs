namespace Remby.Application.CQRS.Responses.Card;

public class CardResponse
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Hint { get; private set; } = string.Empty;
    public string Decision { get; private set; } = string.Empty;
}
