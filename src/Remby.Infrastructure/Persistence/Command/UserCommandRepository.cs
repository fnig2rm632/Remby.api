using Dapper;
using Npgsql;
using Remby.Application.Interfaces.Command;
using Remby.Domain.Common;
using Remby.Domain.Entities;

namespace Remby.Infrastructure.Persistence.Command;

public class UserCommandRepository(NpgsqlConnection connection) : IUserCommandRepository
{
    public async Task<Result> Add(User user)
    {
        try
        {
            const string command = @"insert into users(id, login, last_visit) 
                                     values(@id, @login, @lastVisit)";
            
            var parameters = new DynamicParameters();
            
            parameters.Add("id", user.Id);
            parameters.Add("login", user.Login);
            parameters.Add("lastVisit", user.LastVisit);

            var result = await connection.ExecuteAsync(command, parameters);
            
            if (result == 0)
                return Error.Database.NoCompleted;
            return Result.Success();
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
