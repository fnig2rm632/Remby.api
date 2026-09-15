using MediatR;
using Remby.Application.CQRS.Responses.Folder;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Folder.GetFolder;

public class GetFolderQueryHandler(IFolderQueryRepository repository)
    : IRequestHandler<GetFolderQuery, Result<FolderResponse>>
{
    public async Task<Result<FolderResponse>> Handle(GetFolderQuery request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            return Error.Folder.EmptyId;

        var folder = await repository.GetFolderById(request.Id, cancellationToken);

        return folder;
    }
}
