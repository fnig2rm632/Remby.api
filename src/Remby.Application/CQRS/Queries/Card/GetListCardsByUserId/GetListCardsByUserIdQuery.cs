using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetListCardsByUserId;

public record GetListCardsByUserIdQuery(string UserId) : IRequest<Result<List<CardShortResponse>>>;
