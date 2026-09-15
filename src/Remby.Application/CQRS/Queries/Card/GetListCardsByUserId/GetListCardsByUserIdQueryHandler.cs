using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetListCardsByUserId;

public class GetListCardsByUserIdQueryHandler(ICardQueryRepository repository)
    : IRequestHandler<GetListCardsByUserIdQuery, Result<List<CardShortResponse>>>
{
    public async Task<Result<List<CardShortResponse>>> Handle(GetListCardsByUserIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            return Error.Card.UserIdIsEmpty;

        var cards = await repository.GetListCardsByUserId(request.UserId, cancellationToken);

        return cards;
    }
}
