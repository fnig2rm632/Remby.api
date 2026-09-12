using Dapper;
using Npgsql;
using Remby.Application.Interfaces.Query;
using Remby.Application.SQRS.Responses.Folder;
using Remby.Domain.Common;

namespace Remby.Infrastructure.Persistence.Query;

public class FolderQueryRepository(NpgsqlConnection connection) : IFolderQueryRepository
{
    public async Task<Result<FolderResponse>> GetFolderById(int id)
    {
        try
        {
            const string query = @"select * 
                                   from folders
                                   where delete_at is null
                                   and id = @id";

            var folder = await connection.QuerySingleOrDefaultAsync<FolderResponse>
                (query, new { Id = id });

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

    public async Task<Result<List<FolderShortResponse>>> GetListFoldersByUser(string userId)
    {
        try
        {
            const string query = @"select * 
                                   from folders 
                                   where delete_at is null
                                   and user_id = @userId::uuid";
            
            var folders = await connection.QueryAsync<FolderShortResponse>
                (query, new { UserId = userId });
            
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
