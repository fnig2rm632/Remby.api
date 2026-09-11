using Remby.Domain.Common;
using Remby.Domain.Entities;
using Xunit;

namespace Remby.UnitTests.Domain;

public class FolderTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = 1;
        var name = "Folder 1";
        var description = "Description";
        var userId = Guid.NewGuid();

        var result = Folder.Create(id, name, description, userId);

        Assert.True(result.IsSuccess);
        var folder = result.Value;
        Assert.NotNull(folder);
        Assert.Equal(id, folder.Id);
        Assert.Equal(name, folder.Name);
        Assert.Equal(description, folder.Description);
        Assert.Equal(userId, folder.UserId);
    }

    [Fact]
    public void Create_WithoutId_Valid()
    {
        var name = "Folder 1";
        var description = "Description";
        var userId = Guid.NewGuid();

        var result = Folder.Create(name, description, userId);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value!.Id);
    }

    [Fact]
    public void Create_EmptyName_ReturnsError()
    {
        var id = 1;
        var name = "";
        var description = "Description";
        var userId = Guid.NewGuid();

        var result = Folder.Create(id, name, description, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Folder.EmptyName, result.Error);
    }

    [Fact]
    public void Create_EmptyUserId_ReturnsError()
    {
        var id = 1;
        var name = "Folder 1";
        var description = "Description";
        var userId = Guid.Empty;

        var result = Folder.Create(id, name, description, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Folder.EmptyUserId, result.Error);
    }

    [Fact]
    public void Create_NameTooLong_ReturnsError()
    {
        var id = 1;
        var name = new string('a', 51);
        var description = "Description";
        var userId = Guid.NewGuid();

        var result = Folder.Create(id, name, description, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Folder.NameTooLong(50), result.Error);
    }

    [Fact]
    public void Create_DescriptionTooLong_ReturnsError()
    {
        var id = 1;
        var name = "Folder 1";
        var description = new string('a', 256);
        var userId = Guid.NewGuid();

        var result = Folder.Create(id, name, description, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Folder.DescriptionTooLong(255), result.Error);
    }

    [Fact]
    public void Delete_Valid_UpdatesDeleteAt()
    {
        var id = 1;
        var name = "Folder 1";
        var description = "Description";
        var userId = Guid.NewGuid();
        var deleteAt = DateTime.UtcNow.AddMinutes(-1);

        var folder = Folder.Create(id, name, description, userId).Value!;
        var result = folder.Delete(deleteAt);

        Assert.True(result.IsSuccess);
        Assert.Equal(deleteAt, folder.DeleteAt);
    }

    [Fact]
    public void Delete_InFuture_ReturnsError()
    {
        var id = 1;
        var name = "Folder 1";
        var description = "Description";
        var userId = Guid.NewGuid();
        var deleteAt = DateTime.UtcNow.AddMinutes(1);

        var folder = Folder.Create(id, name, description, userId).Value!;
        var result = folder.Delete(deleteAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Folder.DataInFuture, result.Error);
    }
}
