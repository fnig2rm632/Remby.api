using Remby.Domain.Common;
using Remby.Domain.Entities;
using Remby.Infrastructure.Persistence.Command;
using Xunit;

namespace Remby.IntegrationTests.Repositories.Command;

[Collection("Integration Tests")]
public class CardCommandRepositoryTest(TestInfrastructureFixture fixture)
{
    [Fact]
    public async Task Create_ValidCard_InsertsRow()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var title = "Title";
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, 1, "Folder 1", userId);
        var card = Card.Create(title, "Hint", "Decision", userId, 1).Value!;
        var repository = new CardCommandRepository(connection);

        var result = await repository.Create(card, CancellationToken.None);

        Assert.True(result.IsSuccess);
        var cardId = result.Value;
        Assert.True(cardId > 0);
        Assert.Equal(title, await TestData.GetCardTitleAsync(connection, cardId));
    }

    [Fact]
    public async Task Create_WithoutFolder_InsertsRow()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var title = "Title";
        await TestData.AddUserAsync(connection, userId, "user");
        var card = Card.Create(title, "Hint", "Decision", userId).Value!;
        var repository = new CardCommandRepository(connection);

        var result = await repository.Create(card, CancellationToken.None);

        Assert.True(result.IsSuccess);
        var cardId = result.Value;
        Assert.True(cardId > 0);
        Assert.Equal(title, await TestData.GetCardTitleAsync(connection, cardId));
    }

    [Fact]
    public async Task Update_ExistingCard_Updates()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var cardId = 1;
        var newTitle = "New title";
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddCardAsync(connection, cardId, "Title", userId);
        var card = Card.Create(cardId, newTitle, "Hint", "Decision", userId).Value!;
        var repository = new CardCommandRepository(connection);

        var result = await repository.Update(card, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(newTitle, await TestData.GetCardTitleAsync(connection, cardId));
    }

    [Fact]
    public async Task Update_NotExistingCard_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var card = Card.Create(1, "Title", "Hint", "Decision", Guid.NewGuid()).Value!;
        var repository = new CardCommandRepository(connection);

        var result = await repository.Update(card, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Database.NoCompleted, result.Error);
    }

    [Fact]
    public async Task UpdateRank_ExistingCard_UpdatesRank()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var cardId = 1;
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddCardAsync(connection, cardId, "Title", userId);
        var repository = new CardCommandRepository(connection);

        var result = await repository.UpdateRank(cardId, 3, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(3, await TestData.GetCardRankIdAsync(connection, cardId));
    }

    [Fact]
    public async Task UpdateRank_NotExistingCard_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var repository = new CardCommandRepository(connection);

        var result = await repository.UpdateRank(1, 3, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Database.NoCompleted, result.Error);
    }

    [Fact]
    public async Task UpdateTimeDelete_ExistingCard_SetsDeleteAt()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var cardId = 1;
        var timeDeleted = TestData.DeleteTime;
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddCardAsync(connection, cardId, "Title", userId);
        var repository = new CardCommandRepository(connection);

        var result = await repository.UpdateTimeDelete(cardId, timeDeleted, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(timeDeleted, await TestData.GetCardDeleteAtAsync(connection, cardId));
    }

    [Fact]
    public async Task UpdateTimeDelete_NotExistingCard_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var repository = new CardCommandRepository(connection);

        var result = await repository.UpdateTimeDelete(1, TestData.DeleteTime, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Database.NoCompleted, result.Error);
    }
}
