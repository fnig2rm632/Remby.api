using Remby.Application.SQRS.Responses.Folder;
using Remby.Domain.Common;

namespace Remby.Application.Interfaces.Query;

public interface IFolderQueryRepository
{
    Task<Result<FolderResponse>> GetFolderById(int id);
    Task<Result<List<FolderShortResponse>>> GetListFoldersByUser(string userId);
}