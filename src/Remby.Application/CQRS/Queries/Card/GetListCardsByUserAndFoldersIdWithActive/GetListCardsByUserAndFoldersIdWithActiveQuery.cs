using MediatR;
using Remby.Application.CQRS.Responses.Card;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Card.GetListCardsByUserAndFoldersIdWithActive;

public record GetListCardsByUserAndFoldersIdWithActiveQuery(string UserId, List<int> FolderIds) 
    : IRequest<Result<List<CardShortWithActiveResponse>>>;
