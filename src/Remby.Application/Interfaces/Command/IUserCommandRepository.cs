using Remby.Domain.Common;
using Remby.Domain.Entities;

namespace Remby.Application.Interfaces.Command;

public interface IUserCommandRepository
{
    Task<Result> Add(User user);
}