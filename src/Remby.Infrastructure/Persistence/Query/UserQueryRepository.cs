using Dapper;
using Npgsql;
using Remby.Application.Interfaces.Query;
using Remby.Application.SQRS.Responses.User;
using Remby.Domain.Common;

namespace Remby.Infrastructure.Persistence.Query;

public class UserQueryRepository(NpgsqlConnection connection) : IUserQueryRepository
{
    public async Task<Result<UserResponse>> GetUserByGuid(string guid)
    {
        try
        {
            const string query = @"select * 
                                   from users
                                   where id = @guid::uuid";

            var user = await connection.QuerySingleOrDefaultAsync<UserResponse>
                (query, new { Guid = guid });

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
