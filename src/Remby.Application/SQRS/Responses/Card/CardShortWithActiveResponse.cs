namespace Remby.Application.SQRS.Responses.Card;

public class CardShortWithActiveResponse
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public DateTime LastRepeat { get; private set; }
    public int TimeRepeat { get; private set; }
}