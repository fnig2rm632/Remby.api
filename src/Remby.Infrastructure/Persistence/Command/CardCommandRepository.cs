using Dapper;
using Npgsql;
using Remby.Application.Interfaces.Command;
using Remby.Domain.Common;
using Remby.Domain.Entities;

namespace Remby.Infrastructure.Persistence.Command;

public class CardCommandRepository(NpgsqlConnection connection) : ICardCommandRepository
{
    public async Task<Result<int>> Create(Card card)
    {
        try
        {
            const string command = @"insert into cards(title, hint, decision, folder_id, rank_id, user_id) 
                                     values(@title, @hint, @decision, @folderId, @rankId, @userId)
                                     returning id";
            
            var parameters = new DynamicParameters();
            
            parameters.Add("title", card.Title);
            parameters.Add("hint", card.Hint);
            parameters.Add("decision", card.Decision);
            parameters.Add("folderId", card.FolderId > 0 ? card.FolderId : (int?)null);
            parameters.Add("rankId", card.RankId);
            parameters.Add("userId", card.UserId);

            var id = await connection.ExecuteScalarAsync<int>(command, parameters);
            
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

    public async Task<Result> Update(Card card)
    {
        try
        {
            const string command = @"update cards
                                     set title = @title,
                                         hint = @hint,
                                         decision = @decision,
                                         folder_id = @folderId
                                     where delete_at is null 
                                     and id = @id";
        
            var parameters = new DynamicParameters();
        
            parameters.Add("title", card.Title);
            parameters.Add("hint", card.Hint);
            parameters.Add("decision", card.Decision);
            parameters.Add("folderId", card.FolderId > 0 ? card.FolderId : (int?)null);
            parameters.Add("id", card.Id);
        
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

    public async Task<Result> UpdateRank(int cardId, int rank)
    {
        try
        {
            const string command = @"update cards
                                     set rank_id = @rankId
                                     where delete_at is null 
                                     and id = @id";
        
            var parameters = new DynamicParameters();
        
            parameters.Add("id", cardId);
            parameters.Add("rankId", rank);
        
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

    public async Task<Result> UpdateTimeDelete(int cardId, DateTime timeDeleted)
    {
        try
        {
            const string command = @"update cards
                                     set delete_at = @deleteAt
                                     where delete_at is null 
                                     and id = @id";
        
            var parameters = new DynamicParameters();
        
            parameters.Add("id", cardId);
            parameters.Add("deleteAt", timeDeleted);
        
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
