using Remby.Domain.Common;
using Remby.Domain.Entities;

namespace Remby.Application.Interfaces.Command;

public interface ICardCommandRepository
{
    Task<Result> Create(Card card);
    Task<Result> Update(Card card);
    Task<Result> UpdateRank(int cardId, int rank);
    Task<Result> UpdateTimeDelete(int cardId, DateTime timeDelete);
}