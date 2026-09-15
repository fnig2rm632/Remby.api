using Dapper;
using Npgsql;
using Remby.Application.CQRS.Responses.User;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Infrastructure.Persistence.Query;

public class UserQueryRepository(NpgsqlConnection connection) : IUserQueryRepository
{
    public async Task<Result<UserResponse>> GetUserByGuid(Guid guid, CancellationToken token)
    {
        try
        {
            const string query = @"select * 
                                   from users
                                   where id = @guid";

            var command = new CommandDefinition(
                query,
                new { Guid = guid },
                cancellationToken: token);

            var user = await connection.QuerySingleOrDefaultAsync<UserResponse>(command);

            if (user == null)
                return Error.User.UserNotFound;
            
            return user;
        }
        catch (NpgsqlException)
        {
            return Error.Database.ConnectionFailed;
        }
        catch (TimeoutException)
        {
            return Error.Database.TimeoutGateway;
        }
        catch (Exception)
        {
            return Error.Database.InternalServer;
        }
    }
}
