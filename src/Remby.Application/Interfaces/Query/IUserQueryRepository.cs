using Remby.Application.CQRS.Responses.User;
using Remby.Domain.Common;

namespace Remby.Application.Interfaces.Query;

public interface IUserQueryRepository
{
    Task<Result<UserResponse>> GetUserByGuid(Guid guid, CancellationToken token);
}
