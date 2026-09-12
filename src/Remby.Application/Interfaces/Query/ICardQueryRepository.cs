using Remby.Application.SQRS.Responses.Card;
using Remby.Domain.Common;

namespace Remby.Application.Interfaces.Query;

public interface ICardQueryRepository
{
    Task<Result<CardResponse>> GetCardById(int? cardId);
    Task<Result<List<CardShortResponse>>> GetListCardsByUserId(string userId);
    Task<Result<List<CardShortWithActiveResponse>>> GetListCardsByUserIdWithActive(string userId);
    Task<Result<List<CardShortResponse>>> GetListCardsByUserAndFoldersId(string userId, List<int> folderIds);
    Task<Result<List<CardShortWithActiveResponse>>> 
        GetListCardsByUserAndFoldersIdWithActive(string userId, List<int> folderIds);
}