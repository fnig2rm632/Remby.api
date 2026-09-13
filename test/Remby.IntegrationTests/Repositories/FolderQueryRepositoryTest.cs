using Remby.Domain.Common;
using Remby.Infrastructure.Persistence.Query;
using Xunit;

namespace Remby.IntegrationTests.Repositories;

[Collection("Integration Tests")]
public class FolderQueryRepositoryTest(TestInfrastructureFixture fixture)
{
    [Fact]
    public async Task GetFolderById_ExistingFolder_ReturnsFolder()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var folderId = 1;
        var folderName = "Folder 1";
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, folderId, folderName, userId);
        var repository = new FolderQueryRepository(connection);

        var result = await repository.GetFolderById(folderId);

        Assert.True(result.IsSuccess);
        var folder = result.Value;
        Assert.NotNull(folder);
        Assert.Equal(folderId, folder.Id);
        Assert.Equal(folderName, folder.Name);
    }

    [Fact]
    public async Task GetFolderById_NotExistingFolder_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var repository = new FolderQueryRepository(connection);

        var result = await repository.GetFolderById(1);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Folder.FolderNotFound, result.Error);
    }

    [Fact]
    public async Task GetFolderById_DeletedFolder_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, 1, "Folder 1", userId, deleteAt: DateTime.UtcNow);
        var repository = new FolderQueryRepository(connection);

        var result = await repository.GetFolderById(1);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Folder.FolderNotFound, result.Error);
    }

    [Fact]
    public async Task GetListFoldersByUser_ExistingFolders_ReturnsList()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, 1, "Folder 1", userId);
        await TestData.AddFolderAsync(connection, 2, "Folder 2", userId);
        var repository = new FolderQueryRepository(connection);

        var result = await repository.GetListFoldersByUser(userId.ToString());

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task GetListFoldersByUser_NotExistingFolders_ReturnsEmptyList()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var repository = new FolderQueryRepository(connection);

        var result = await repository.GetListFoldersByUser(Guid.NewGuid().ToString());

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task GetListFoldersByUser_DeletedFolder_Excluded()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        await TestData.AddUserAsync(connection, userId, "user");
        await TestData.AddFolderAsync(connection, 1, "Folder 1", userId);
        await TestData.AddFolderAsync(connection, 2, "Folder 2", userId, deleteAt: DateTime.UtcNow);
        var repository = new FolderQueryRepository(connection);

        var result = await repository.GetListFoldersByUser(userId.ToString());

        Assert.True(result.IsSuccess);
        var folder = Assert.Single(result.Value!);
        Assert.Equal(1, folder.Id);
    }
}
