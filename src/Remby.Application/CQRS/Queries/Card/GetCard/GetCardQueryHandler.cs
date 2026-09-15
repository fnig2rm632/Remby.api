using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetCard;

public class GetCardQueryHandler(ICardQueryRepository repository)
    : IRequestHandler<GetCardQuery, Result<CardResponse>>
{
    public async Task<Result<CardResponse>> Handle(GetCardQuery request, CancellationToken cancellationToken)
    {
        if (request.Id is null or <= 0)
            return Error.Card.EmptyId;

        var card = await repository.GetCardById(request.Id, cancellationToken);

        return card;
    }
}
