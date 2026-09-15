using Remby.Domain.Common;
using Remby.Domain.Entities;

namespace Remby.Application.Interfaces.Command;

public interface IFolderCommandRepository
{
    Task<Result<int>> Add(Folder folder, CancellationToken token);
    Task<Result> Update(Folder folder, CancellationToken token);
    Task<Result> UpdateTimeDelete(int folderId, DateTime timeDelete, CancellationToken token);
}
