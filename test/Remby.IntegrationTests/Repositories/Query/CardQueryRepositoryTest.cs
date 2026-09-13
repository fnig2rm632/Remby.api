using Remby.Domain.Common;
using Remby.Infrastructure.Persistence.Query;
using Xunit;

namespace Remby.IntegrationTests.Repositories.Query;

[Collection("Integration Tests")]
public class CardQueryRepositoryTest(TestInfrastructureFixture fixture)
{
    [Fact]
    public async Task GetCardById_ExistingCard_ReturnsCard()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var cardId = 1;
        var title = "Title";
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddCardAsync(connection, cardId, title, userId);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetCardById(cardId);

        Assert.True(result.IsSuccess);
        var card = result.Value;
        Assert.NotNull(card);
        Assert.Equal(cardId, card.Id);
        Assert.Equal(title, card.Title);
    }

    [Fact]
    public async Task GetCardById_NotExistingCard_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetCardById(1);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.CardNotFound, result.Error);
    }

    [Fact]
    public async Task GetCardById_DeletedCard_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddCardAsync(connection, 1, "Title", userId, deleteAt: DateTime.UtcNow);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetCardById(1);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.CardNotFound, result.Error);
    }

    [Fact]
    public async Task GetListCardsByUserId_ExistingCards_ReturnsList()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddCardAsync(connection, 1, "Title 1", userId);
        await TestData.AddCardAsync(connection, 2, "Title 2", userId);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetListCardsByUserId(userId.ToString());

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task GetListCardsByUserId_NotExistingCards_ReturnsEmptyList()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetListCardsByUserId(Guid.NewGuid().ToString());

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task GetListCardsByUserId_DeletedCard_Excluded()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddCardAsync(connection, 1, "Title 1", userId);
        await TestData.AddCardAsync(connection, 2, "Title 2", userId, deleteAt: DateTime.UtcNow);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetListCardsByUserId(userId.ToString());

        Assert.True(result.IsSuccess);
        var card = Assert.Single(result.Value!);
        Assert.Equal(1, card.Id);
    }

    [Fact]
    public async Task GetListCardsByUserIdWithActive_ExistingCards_ReturnsActive()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddCardAsync(connection, 1, "Title 1", userId);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetListCardsByUserIdWithActive(userId.ToString());

        Assert.True(result.IsSuccess);
        var card = Assert.Single(result.Value!);
        Assert.Equal(1, card.Id);
        Assert.Equal(TestData.LastRepeatTime, card.LastRepeat);
        Assert.Equal(5, card.TimeRepeat);
    }

    [Fact]
    public async Task GetListCardsByUserAndFoldersId_ExistingCards_ReturnsFiltered()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, 1, "Folder 1", userId);
        await TestData.AddFolderAsync(connection, 2, "Folder 2", userId);
        await TestData.AddCardAsync(connection, 1, "Title 1", userId, folderId: 1);
        await TestData.AddCardAsync(connection, 2, "Title 2", userId, folderId: 2);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetListCardsByUserAndFoldersId(userId.ToString(), [1]);

        Assert.True(result.IsSuccess);
        var card = Assert.Single(result.Value!);
        Assert.Equal(1, card.Id);
    }

    [Fact]
    public async Task GetListCardsByUserAndFoldersIdWithActive_ExistingCards_ReturnsFiltered()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, 1, "Folder 1", userId);
        await TestData.AddFolderAsync(connection, 2, "Folder 2", userId);
        await TestData.AddCardAsync(connection, 1, "Title 1", userId, folderId: 1);
        await TestData.AddCardAsync(connection, 2, "Title 2", userId, folderId: 2);
        var repository = new CardQueryRepositor(connection);

        var result = await repository.GetListCardsByUserAndFoldersIdWithActive(userId.ToString(), [1]);

        Assert.True(result.IsSuccess);
        var card = Assert.Single(result.Value!);
        Assert.Equal(1, card.Id);
        Assert.Equal(5, card.TimeRepeat);
    }
}
