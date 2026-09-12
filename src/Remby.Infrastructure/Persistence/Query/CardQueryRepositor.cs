using Dapper;
using Npgsql;
using Remby.Application.Interfaces.Query;
using Remby.Application.SQRS.Responses.Card;
using Remby.Domain.Common;

namespace Remby.Infrastructure.Persistence.Query;

public class CardQueryRepositor(NpgsqlConnection connection) : ICardQueryRepository
{
    public async Task<Result<CardResponse>> GetCardById(int? id)
    {
        const string query = @"select * 
                               from cards 
                               where delete_at is null
                               and id = @id";
        
        var card = await connection.QuerySingleOrDefaultAsync<CardResponse>
            (query, new { Id = id });
        
        if (card == null)
            return Error.Card.CardNotFound;

        return card;
    }

    public async Task<Result<List<CardShortResponse>>> GetListCardsByUserId(string userId)
    {
        const string query = @"select id, title 
                               from cards 
                               where delete_at is null
                               and user_id = @userId::uuid";
        
        var cards = await connection.QueryAsync<CardShortResponse>
            (query, new { UserId = userId });
        
        return cards.ToList();
    }

    public async Task<Result<List<CardShortWithActiveResponse>>> GetListCardsByUserIdWithActive(string userId)
    {
        const string query = @"select cards.id, 
                                      cards.title, 
                                      cards.last_repeat as ""LastRepeat"", 
                                      ranks.time_repeat as ""TimeRepeat""
                               from cards 
                               join ranks on ranks.id = cards.rank_id
                               where cards.delete_at is null
                               and cards.user_id = @userId::uuid";
        
        var cards = await connection.QueryAsync<CardShortWithActiveResponse>
            (query, new { UserId = userId });
        
        return cards.ToList();
    }

    public async Task<Result<List<CardShortResponse>>> GetListCardsByUserAndFoldersId(string userId, List<int> folderIds)
    {
        const string query = @"select id, title 
                               from cards 
                               where delete_at is null
                               and folder_id = any(@folderIds)
                               and user_id = @userId::uuid";

        var parameters = new DynamicParameters();
        parameters.Add("folderIds", folderIds.ToArray());
        parameters.Add("userId", userId);
        
        var cards = await connection.QueryAsync<CardShortResponse>
            (query, parameters);
        
        return cards.ToList();
    }

    public async Task<Result<List<CardShortWithActiveResponse>>> GetListCardsByUserAndFoldersIdWithActive(string userId, List<int> folderIds)
    {
        const string query = @"select cards.id, 
                                      cards.title, 
                                      cards.last_repeat as ""LastRepeat"", 
                                      ranks.time_repeat as ""TimeRepeat""
                               from cards 
                               join ranks on ranks.id = cards.rank_id
                               where cards.delete_at is null
                               and cards.folder_id = any(@folderIds)
                               and cards.user_id = @userId::uuid";

        var parameters = new DynamicParameters();
        parameters.Add("folderIds", folderIds.ToArray());
        parameters.Add("userId", userId);
        
        var cards = await connection.QueryAsync<CardShortWithActiveResponse>
            (query, parameters);
        
        return cards.ToList();
    }
}
