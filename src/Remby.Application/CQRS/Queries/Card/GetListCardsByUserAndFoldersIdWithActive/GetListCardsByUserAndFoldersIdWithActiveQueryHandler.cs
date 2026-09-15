using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetListCardsByUserAndFoldersIdWithActive;

public class GetListCardsByUserAndFoldersIdWithActiveQueryHandler(ICardQueryRepository repository)
    : IRequestHandler<GetListCardsByUserAndFoldersIdWithActiveQuery, Result<List<CardShortWithActiveResponse>>>
{
    public async Task<Result<List<CardShortWithActiveResponse>>> Handle(GetListCardsByUserAndFoldersIdWithActiveQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            return Error.Card.UserIdIsEmpty;

        if (request.FolderIds.Count == 0)
            return Error.Card.EmptyFolderIds;

        var cards = await repository.GetListCardsByUserAndFoldersIdWithActive(request.UserId, request.FolderIds, cancellationToken);

        return cards;
    }
}
