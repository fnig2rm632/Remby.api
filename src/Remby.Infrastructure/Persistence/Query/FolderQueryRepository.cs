using Dapper;
using Npgsql;
using Remby.Application.CQRS.Responses.Folder;
using Remby.Application.Interfaces.Query;
using Remby.Domain.Common;

namespace Remby.Infrastructure.Persistence.Query;

public class FolderQueryRepository(NpgsqlConnection connection) : IFolderQueryRepository
{
    public async Task<Result<FolderResponse>> GetFolderById(int id, CancellationToken token)
    {
        try
        {
            const string query = @"select * 
                                   from folders
                                   where delete_at is null
                                   and id = @id";

            var command = new CommandDefinition(
                query,
                new { Id = id },
                cancellationToken: token);

            var folder = await connection.QuerySingleOrDefaultAsync<FolderResponse>(command);

            if (folder == null)
                return Error.Folder.FolderNotFound;
            
            return folder;
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

    public async Task<Result<List<FolderShortResponse>>> GetListFoldersByUser(string userId, CancellationToken token)
    {
        try
        {
            const string query = @"select * 
                                   from folders 
                                   where delete_at is null
                                   and user_id = @userId::uuid";
            
            var command = new CommandDefinition(
                query,
                new { UserId = userId },
                cancellationToken: token);

            var folders = await connection.QueryAsync<FolderShortResponse>(command);
            
            return folders.ToList();
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
