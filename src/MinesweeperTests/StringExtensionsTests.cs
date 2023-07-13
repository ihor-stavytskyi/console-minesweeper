using FluentAssertions;

namespace MinesweeperGameTests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("s", 3, " s ")]
    [InlineData("s", 4, " s  ")]
    [InlineData("text", 1, "text")]
    [InlineData("text", 4, "text")]
    public void AlignToCenterShouldWork(string input, int width, string expectedString)
    {
        var aligned = input.AlignToCenter(width);
        aligned.Should().Be(expectedString);
    }
}