using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetCard;

public record GetCardQuery(int? Id) : IRequest<Result<CardResponse>>;
