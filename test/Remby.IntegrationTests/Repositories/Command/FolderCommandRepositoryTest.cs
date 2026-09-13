using Remby.Domain.Common;
using Remby.Domain.Entities;
using Remby.Infrastructure.Persistence.Command;
using Xunit;

namespace Remby.IntegrationTests.Repositories.Command;

[Collection("Integration Tests")]
public class FolderCommandRepositoryTest(TestInfrastructureFixture fixture)
{
    [Fact]
    public async Task Add_ValidFolder_InsertsRow()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var name = "Folder 1";
        await TestData.AddUserAsync(connection, userId, "user");
        var folder = Folder.Create(name, "Description", userId).Value!;
        var repository = new FolderCommandRepository(connection);

        var result = await repository.Add(folder);

        Assert.True(result.IsSuccess);
        var folderId = result.Value;
        Assert.True(folderId > 0);
        Assert.Equal(name, await TestData.GetFolderNameAsync(connection, folderId));
    }

    [Fact]
    public async Task Update_ExistingFolder_Updates()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var folderId = 1;
        var newName = "New name";
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, folderId, "Folder 1", userId);
        var folder = Folder.Create(folderId, newName, "New description", userId).Value!;
        var repository = new FolderCommandRepository(connection);

        var result = await repository.Update(folder);

        Assert.True(result.IsSuccess);
        Assert.Equal(newName, await TestData.GetFolderNameAsync(connection, folderId));
    }

    [Fact]
    public async Task Update_NotExistingFolder_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var folder = Folder.Create(1, "Folder 1", "Description", Guid.NewGuid()).Value!;
        var repository = new FolderCommandRepository(connection);

        var result = await repository.Update(folder);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Database.NoCompleted, result.Error);
    }

    [Fact]
    public async Task Update_DeletedFolder_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var folderId = 1;
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, folderId, "Folder 1", userId, deleteAt: DateTime.UtcNow);
        var folder = Folder.Create(folderId, "New name", "New description", userId).Value!;
        var repository = new FolderCommandRepository(connection);

        var result = await repository.Update(folder);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Database.NoCompleted, result.Error);
    }

    [Fact]
    public async Task UpdateTimeDelete_ExistingFolder_SetsDeleteAt()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var folderId = 1;
        var timeDeleted = TestData.DeleteTime;
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, folderId, "Folder 1", userId);
        var repository = new FolderCommandRepository(connection);

        var result = await repository.UpdateTimeDelete(folderId, timeDeleted);

        Assert.True(result.IsSuccess);
        Assert.Equal(timeDeleted, await TestData.GetFolderDeleteAtAsync(connection, folderId));
    }

    [Fact]
    public async Task UpdateTimeDelete_NotExistingFolder_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var repository = new FolderCommandRepository(connection);

        var result = await repository.UpdateTimeDelete(1, TestData.DeleteTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Database.NoCompleted, result.Error);
    }
}
