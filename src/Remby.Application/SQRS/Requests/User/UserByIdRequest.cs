using MediatR;
using Remby.Application.SQRS.Responses.User;
using Remby.Domain.Common;

namespace Remby.Application.SQRS.Requests.User;

public class UserByIdRequest : IRequest<Result<UserResponse>>
{
    public string UserId { get; set; } = null!;
}