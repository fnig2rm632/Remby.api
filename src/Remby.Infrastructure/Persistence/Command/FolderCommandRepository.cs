using Dapper;
using Npgsql;
using Remby.Application.Interfaces.Command;
using Remby.Domain.Common;
using Remby.Domain.Entities;

namespace Remby.Infrastructure.Persistence.Command;

public class FolderCommandRepository(NpgsqlConnection connection) : IFolderCommandRepository
{
    public async Task<Result<int>> Add(Folder folder, CancellationToken token)
    {
        try
        {
            const string sql = @"insert into folders(name, description, user_id) 
                                 values(@name, @description, @userId)
                                 returning id";
            
            var parameters = new DynamicParameters();
            
            parameters.Add("name", folder.Name);
            parameters.Add("description", folder.Description);
            parameters.Add("userId", folder.UserId);

            var command = new CommandDefinition(
                sql,
                parameters,
                cancellationToken: token);

            var id = await connection.ExecuteScalarAsync<int>(command);
            
            if (id == 0)
                return Error.Database.NoCompleted;
            return id;
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

    public async Task<Result> Update(Folder folder, CancellationToken token)
    {
        try
        {
            const string sql = @"update folders
                                 set name = @name,
                                     description = @description
                                 where delete_at is null 
                                 and id = @id";
        
            var parameters = new DynamicParameters();
            parameters.Add("name", folder.Name);
            parameters.Add("description", folder.Description);
            parameters.Add("id", folder.Id);

            var command = new CommandDefinition(
                sql,
                parameters,
                cancellationToken: token);

            var result = await connection.ExecuteAsync(command);

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

    public async Task<Result> UpdateTimeDelete(int folderId, DateTime timeDeleted, CancellationToken token)
    {
        try
        {
            const string sql = @"update folders
                                 set delete_at = @deleteAt
                                 where delete_at is null 
                                 and id = @id";
        
            var parameters = new DynamicParameters();
            parameters.Add("id", folderId);
            parameters.Add("deleteAt", timeDeleted);

            var command = new CommandDefinition(
                sql,
                parameters,
                cancellationToken: token);

            var result = await connection.ExecuteAsync(command);

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
