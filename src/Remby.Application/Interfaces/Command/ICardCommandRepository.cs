using Remby.Domain.Common;
using Remby.Domain.Entities;

namespace Remby.Application.Interfaces.Command;

public interface ICardCommandRepository
{
    Task<Result<int>> Create(Card card, CancellationToken token);
    Task<Result> Update(Card card, CancellationToken token);
    Task<Result> UpdateRank(int cardId, int rank, CancellationToken token);
    Task<Result> UpdateTimeDelete(int cardId, DateTime timeDelete, CancellationToken token);
}
