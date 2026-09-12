using Remby.Application.SQRS.Responses.User;
using Remby.Domain.Common;

namespace Remby.Application.Interfaces.Query;

public interface IUserQueryRepository
{
    Task<Result<UserResponse>> GetUserByGuid(string guid);
}