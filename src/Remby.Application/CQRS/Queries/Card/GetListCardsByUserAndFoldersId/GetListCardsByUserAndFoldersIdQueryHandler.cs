using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetListCardsByUserAndFoldersId;

public class GetListCardsByUserAndFoldersIdQueryHandler(ICardQueryRepository repository)
    : IRequestHandler<GetListCardsByUserAndFoldersIdQuery, Result<List<CardShortResponse>>>
{
    public async Task<Result<List<CardShortResponse>>> Handle(GetListCardsByUserAndFoldersIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            return Error.Card.UserIdIsEmpty;

        if (request.FolderIds.Count == 0)
            return Error.Card.EmptyFolderIds;

        var cards = await repository.GetListCardsByUserAndFoldersId(request.UserId, request.FolderIds, cancellationToken);

        return cards;
    }
}
