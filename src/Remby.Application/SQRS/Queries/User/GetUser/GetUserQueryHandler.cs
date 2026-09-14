using MediatR;
using Remby.Application.Interfaces.Query;
using Remby.Application.SQRS.Requests.User;
using Remby.Application.SQRS.Responses.User;
using Remby.Domain.Common;

namespace Remby.Application.SQRS.Queries.User.GetUser;

public class GetUserQueryHandler(IUserQueryRepository repository)
    : IRequestHandler<UserByIdRequest, Result<UserResponse>> 
{
    public async Task<Result<UserResponse>>
        Handle(UserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByGuid(request.UserId);
        
        return user;
    }
}