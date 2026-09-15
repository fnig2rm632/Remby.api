using Remby.Application.CQRS.Responses.Card;
using Remby.Domain.Common;

namespace Remby.Application.Interfaces.Query;

public interface ICardQueryRepository
{
    Task<Result<CardResponse>> GetCardById(int? cardId, CancellationToken token);
    Task<Result<List<CardShortResponse>>> GetListCardsByUserId(string userId, CancellationToken token);
    Task<Result<List<CardShortWithActiveResponse>>> GetListCardsByUserIdWithActive(string userId, CancellationToken token);
    Task<Result<List<CardShortResponse>>> 
        GetListCardsByUserAndFoldersId(string userId, List<int> folderIds, CancellationToken token);
    Task<Result<List<CardShortWithActiveResponse>>> 
        GetListCardsByUserAndFoldersIdWithActive(string userId, List<int> folderIds, CancellationToken token);
}
