using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetListCardsByUserIdWithActive;

public record GetListCardsByUserIdWithActiveQuery(string UserId) 
    : IRequest<Result<List<CardShortWithActiveResponse>>>;
