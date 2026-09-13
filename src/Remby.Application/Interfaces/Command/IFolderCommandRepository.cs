using Remby.Domain.Common;
using Remby.Domain.Entities;

namespace Remby.Application.Interfaces.Command;

public interface IFolderCommandRepository
{
    Task<Result<int>> Add(Folder folder);
    Task<Result> Update(Folder folder);
    Task<Result> UpdateTimeDelete(int folderId, DateTime timeDelete);
}
