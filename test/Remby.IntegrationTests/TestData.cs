using Dapper;
using Npgsql;

namespace Remby.IntegrationTests;

internal static class TestData
{
    public static readonly DateTime LastRepeatTime = 
        new(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    public static async Task ClearAsync(NpgsqlConnection connection)
    {
        const string query = @"delete from cards;
                               delete from folders;
                               delete from users;";

        await connection.ExecuteAsync(query);
    }

    public static async Task AddUserAsync(NpgsqlConnection connection, Guid id, string login)
    {
        const string query = 
            @"insert into users (id, login, last_visit)
              values (@id, @login, @lastVisit)";

        await connection.ExecuteAsync(query, new { id, login, lastVisit = DateTime.UtcNow });
    }

    public static async Task AddFolderAsync(NpgsqlConnection connection, int id, string name, Guid userId,
        string description = "Description", DateTime? deleteAt = null)
    {
        const string query = 
            @"insert into folders (id, name, description, user_id, delete_at)
              values (@id, @name, @description, @userId, @deleteAt)";

        await connection.ExecuteAsync(query, new { id, name, description, userId, deleteAt });
    }

    public static async Task AddCardAsync(NpgsqlConnection connection, int id, string title, Guid userId,
        int? folderId = null, int rankId = 1, DateTime? lastRepeat = null, DateTime? deleteAt = null)
    {
        const string query = 
            @"insert into cards (id, title, hint, decision, folder_id, rank_id, user_id, last_repeat, delete_at)
              values (@id, @title, @hint, @decision, @folderId, @rankId, @userId, @lastRepeat, @deleteAt)";

        await connection.ExecuteAsync(query, new
        {
            id,
            title,
            hint = "Hint",
            decision = "Decision",
            folderId,
            rankId,
            userId,
            lastRepeat = lastRepeat ?? LastRepeatTime,
            deleteAt
        });
    }
}
