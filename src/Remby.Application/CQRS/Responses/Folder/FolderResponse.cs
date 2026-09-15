namespace Remby.Application.CQRS.Responses.Folder;

public class FolderResponse
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
}