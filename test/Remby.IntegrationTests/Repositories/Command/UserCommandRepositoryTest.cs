using Remby.Domain.Entities;
using Remby.Infrastructure.Persistence.Command;
using Xunit;

namespace Remby.IntegrationTests.Repositories.Command;

[Collection("Integration Tests")]
public class UserCommandRepositoryTest(TestInfrastructureFixture fixture)
{
    [Fact]
    public async Task Add_ValidUser_InsertsRow()
    {
        await using var connection = await fixture.OpenConnectionAsync();
        await TestData.ClearAsync(connection);
        var userId = Guid.NewGuid();
        var login = "user";
        var user = User.Create(userId, login, DateTime.UtcNow.AddHours(-1)).Value!;
        var repository = new UserCommandRepository(connection);

        var result = await repository.Add(user, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(login, await TestData.GetUserLoginAsync(connection, userId));
    }
}
