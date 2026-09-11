using Remby.Domain.Common;
using Remby.Domain.Entities;
using Xunit;

namespace Remby.UnitTests.Domain;

public class CardTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();

        var result = Card.Create(id, title, hint, decision, userId);

        Assert.True(result.IsSuccess);
        var card = result.Value;
        Assert.NotNull(card);
        Assert.Equal(id, card.Id);
        Assert.Equal(title, card.Title);
        Assert.Equal(hint, card.Hint);
        Assert.Equal(decision, card.Decision);
        Assert.Equal(userId, card.UserId);
    }

    [Fact]
    public void Create_WithFolderId_Valid()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();
        var folderId = 5;

        var result = Card.Create(id, title, hint, decision, userId, folderId);

        Assert.True(result.IsSuccess);
        Assert.Equal(folderId, result.Value!.FolderId);
    }

    [Fact]
    public void Create_EmptyTitle_ReturnsError()
    {
        var id = 1;
        var title = "";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();

        var result = Card.Create(id, title, hint, decision, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.EmptyTitle, result.Error);
    }

    [Fact]
    public void Create_EmptyUserId_ReturnsError()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.Empty;

        var result = Card.Create(id, title, hint, decision, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.UserIdIsEmpty, result.Error);
    }

    [Fact]
    public void Create_TitleTooLong_ReturnsError()
    {
        var id = 1;
        var title = new string('a', 51);
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();

        var result = Card.Create(id, title, hint, decision, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.TitleTooLong(50), result.Error);
    }

    [Fact]
    public void Create_HintTooLong_ReturnsError()
    {
        var id = 1;
        var title = "Title";
        var hint = new string('a', 51);
        var decision = "Decision";
        var userId = Guid.NewGuid();

        var result = Card.Create(id, title, hint, decision, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.HintTooLong(50), result.Error);
    }

    [Fact]
    public void Create_EmptyDecision_ReturnsError()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "";
        var userId = Guid.NewGuid();

        var result = Card.Create(id, title, hint, decision, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.EmptyDecision, result.Error);
    }

    [Fact]
    public void Create_DecisionTooLong_ReturnsError()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = new string('a', 256);
        var userId = Guid.NewGuid();

        var result = Card.Create(id, title, hint, decision, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.DecisionTooLong(255), result.Error);
    }

    [Fact]
    public void Delete_Valid_UpdatesDeleteAt()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();
        var deleteAt = DateTime.UtcNow.AddMinutes(-1);

        var card = Card.Create(id, title, hint, decision, userId).Value!;
        var result = card.Delete(deleteAt);

        Assert.True(result.IsSuccess);
        Assert.Equal(deleteAt, card.DeleteAt);
    }

    [Fact]
    public void Delete_InFuture_ReturnsError()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();
        var deleteAt = DateTime.UtcNow.AddMinutes(1);

        var card = Card.Create(id, title, hint, decision, userId).Value!;
        var result = card.Delete(deleteAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.DeleteAtInFuture, result.Error);
    }

    [Fact]
    public void UpdateLastRepeat_Valid_Updates()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();
        var lastRepeat = DateTime.UtcNow.AddMinutes(-1);

        var card = Card.Create(id, title, hint, decision, userId).Value!;
        var result = card.UpdateLastRepeat(lastRepeat);

        Assert.True(result.IsSuccess);
        Assert.Equal(lastRepeat, card.LastRepeat);
    }

    [Fact]
    public void UpdateLastRepeat_InFuture_ReturnsError()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();
        var lastRepeat = DateTime.UtcNow.AddMinutes(1);

        var card = Card.Create(id, title, hint, decision, userId).Value!;
        var result = card.UpdateLastRepeat(lastRepeat);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Card.LastRepeatInFuture, result.Error);
    }

    [Fact]
    public void ChangeRank_Valid_UpdatesRankId()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();
        var rankId = 3;

        var card = Card.Create(id, title, hint, decision, userId).Value!;
        var result = card.ChangeRank(rankId);

        Assert.True(result.IsSuccess);
        Assert.Equal(rankId, card.RankId);
    }

    [Fact]
    public void ChangeRank_EmptyId_ReturnsError()
    {
        var id = 1;
        var title = "Title";
        var hint = "Hint";
        var decision = "Decision";
        var userId = Guid.NewGuid();
        var rankId = -1;

        var card = Card.Create(id, title, hint, decision, userId).Value!;
        var result = card.ChangeRank(rankId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Rank.EmptyId, result.Error);
    }
}
