using MediatR;
using Remby.Application.SQRS.Responses.User;
using Remby.Domain.Common;

namespace Remby.Application.SQRS.Queries.User.GetUser;

public class GetUserQuery(string guid) : IRequest<Result<UserResponse>>;