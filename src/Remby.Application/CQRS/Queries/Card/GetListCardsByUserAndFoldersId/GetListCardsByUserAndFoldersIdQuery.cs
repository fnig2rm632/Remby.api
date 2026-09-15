using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetListCardsByUserAndFoldersId;

public record GetListCardsByUserAndFoldersIdQuery(string UserId, List<int> FolderIds) 
    : IRequest<Result<List<CardShortResponse>>>;
