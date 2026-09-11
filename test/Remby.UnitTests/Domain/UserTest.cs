using Remby.Domain.Common;
using Remby.Domain.Entities;
using Xunit;

namespace Remby.UnitTests.Domain;

public class UserTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = Guid.NewGuid();
        var login = "user";
        var lastVisit = DateTime.UtcNow.AddHours(-1);

        var result = User.Create(id, login, lastVisit);

        Assert.True(result.IsSuccess);
        var user = result.Value;
        Assert.NotNull(user);
        Assert.Equal(id, user.Id);
        Assert.Equal(login, user.Login);
        Assert.Equal(lastVisit, user.LastVisit);
    }

    [Fact]
    public void Create_WithoutId_Valid()
    {
        var login = "user";
        var lastVisit = DateTime.UtcNow.AddHours(-1);

        var result = User.Create(login, lastVisit);

        Assert.True(result.IsSuccess);
        Assert.Equal(Guid.Empty, result.Value!.Id);
    }

    [Fact]
    public void Create_EmptyLogin_ReturnsError()
    {
        var id = Guid.NewGuid();
        var login = "";
        var lastVisit = DateTime.UtcNow;

        var result = User.Create(id, login, lastVisit);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.EmptyLogin, result.Error);
    }

    [Fact]
    public void Create_VisitInFuture_ReturnsError()
    {
        var id = Guid.NewGuid();
        var login = "user";
        var lastVisit = DateTime.UtcNow.AddHours(1);

        var result = User.Create(id, login, lastVisit);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.DateVisitInFuture, result.Error);
    }

    [Fact]
    public void UpdateLastVisit_Valid_Updates()
    {
        var id = Guid.NewGuid();
        var login = "user";
        var lastVisit = DateTime.UtcNow.AddHours(-2);
        var newLastVisit = DateTime.UtcNow.AddHours(-1);

        var user = User.Create(id, login, lastVisit).Value!;
        var result = user.UpdateLastVisit(newLastVisit);

        Assert.True(result.IsSuccess);
        Assert.Equal(newLastVisit, user.LastVisit);
    }

    [Fact]
    public void UpdateLastVisit_InFuture_ReturnsError()
    {
        var id = Guid.NewGuid();
        var login = "user";
        var lastVisit = DateTime.UtcNow.AddHours(-2);
        var newLastVisit = DateTime.UtcNow.AddHours(1);

        var user = User.Create(id, login, lastVisit).Value!;
        var result = user.UpdateLastVisit(newLastVisit);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.DateVisitInFuture, result.Error);
    }
}
