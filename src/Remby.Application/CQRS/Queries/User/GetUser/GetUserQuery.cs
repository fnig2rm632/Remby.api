using MediatR;
using Remby.Application.CQRS.Responses.User;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.User.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<Result<UserResponse>>;
