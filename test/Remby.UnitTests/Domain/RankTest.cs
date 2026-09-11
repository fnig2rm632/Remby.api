using Remby.Domain.Common;
using Remby.Domain.Entities;
using Xunit;

namespace Remby.UnitTests.Domain;

public class RankTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = 1;
        var name = "Rank 1";
        var timeRepeat = 5;

        var result = Rank.Create(id, name, timeRepeat);

        Assert.True(result.IsSuccess);
        var rank = result.Value;
        Assert.NotNull(rank);
        Assert.Equal(id, rank.Id);
        Assert.Equal(name, rank.Name);
        Assert.Equal(timeRepeat, rank.TimeRepeat);
    }

    [Fact]
    public void Create_EmptyId_ReturnsError()
    {
        var id = 0;
        var name = "Rank 1";
        var timeRepeat = 5;

        var result = Rank.Create(id, name, timeRepeat);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Rank.EmptyId, result.Error);
    }

    [Fact]
    public void Create_EmptyName_ReturnsError()
    {
        var id = 1;
        var name = "";
        var timeRepeat = 5;

        var result = Rank.Create(id, name, timeRepeat);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Rank.EmptyName, result.Error);
    }

    [Fact]
    public void Create_ZeroTimeRepeat_ReturnsError()
    {
        var id = 1;
        var name = "Rank 1";
        var timeRepeat = 0;

        var result = Rank.Create(id, name, timeRepeat);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Rank.MinDate, result.Error);
    }
}
