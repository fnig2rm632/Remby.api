using Remby.Domain.Common;
using Remby.Infrastructure.Persistence.Query;
using Xunit;

namespace Remby.IntegrationTests.Repositories.Query;

[Collection("Integration Tests")]
public class UserQueryRepositoryTest(TestInfrastructureFixture fixture)
{
    [Fact]
    public async Task GetUserByGuid_ExistingUser_ReturnsUser()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var login = "user";
        await TestData.AddUserAsync(connection, userId, login);
        var repository = new UserQueryRepository(connection);

        var result = await repository.GetUserByGuid(userId.ToString());

        Assert.True(result.IsSuccess);
        var user = result.Value;
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        Assert.Equal(login, user.Login);
    }

    [Fact]
    public async Task GetUserByGuid_NotExistingUser_ReturnsError()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var repository = new UserQueryRepository(connection);

        var result = await repository.GetUserByGuid(Guid.NewGuid().ToString());

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.UserNotFound, result.Error);
    }
}
