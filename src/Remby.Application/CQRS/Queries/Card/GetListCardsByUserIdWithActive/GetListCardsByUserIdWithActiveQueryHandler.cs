using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetListCardsByUserIdWithActive;

public class GetListCardsByUserIdWithActiveQueryHandler(ICardQueryRepository repository)
    : IRequestHandler<GetListCardsByUserIdWithActiveQuery, Result<List<CardShortWithActiveResponse>>>
{
    public async Task<Result<List<CardShortWithActiveResponse>>> Handle(GetListCardsByUserIdWithActiveQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            return Error.Card.UserIdIsEmpty;

        var cards = await repository.GetListCardsByUserIdWithActive(request.UserId, cancellationToken);

        return cards;
    }
}
