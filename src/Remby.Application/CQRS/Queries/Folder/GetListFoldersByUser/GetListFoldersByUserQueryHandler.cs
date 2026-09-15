using MediatR;
using Remby.Application.CQRS.Responses.Folder;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Folder.GetListFoldersByUser;

public class GetListFoldersByUserQueryHandler(IFolderQueryRepository repository)
    : IRequestHandler<GetListFoldersByUserQuery, Result<List<FolderShortResponse>>>
{
    public async Task<Result<List<FolderShortResponse>>> Handle(GetListFoldersByUserQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            return Error.Folder.EmptyUserId;

        var folders = await repository.GetListFoldersByUser(request.UserId, cancellationToken);

        return folders;
    }
}
