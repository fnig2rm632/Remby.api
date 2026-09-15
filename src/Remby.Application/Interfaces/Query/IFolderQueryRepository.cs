using Remby.Application.CQRS.Responses.Folder;
using Remby.Domain.Common;

namespace Remby.Application.Interfaces.Query;

public interface IFolderQueryRepository
{
    Task<Result<FolderResponse>> GetFolderById(int id, CancellationToken token);
    Task<Result<List<FolderShortResponse>>> GetListFoldersByUser(string userId, CancellationToken token);
}
