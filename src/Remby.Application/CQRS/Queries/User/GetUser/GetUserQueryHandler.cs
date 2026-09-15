using MediatR;
using Remby.Application.CQRS.Responses.User;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.User.GetUser;

public class GetUserQueryHandler(IUserQueryRepository repository)
    : IRequestHandler<GetUserQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            return Error.User.EmptyId;

        var user = await repository.GetUserByGuid(request.UserId, cancellationToken);

        return user;
    }
}
