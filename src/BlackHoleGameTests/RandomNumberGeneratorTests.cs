using FluentAssertions;

namespace BlackHoleGameTests;

public class RandomNumberGeneratorTests
{
    [Fact]
    public void SampleShouldGenerateUniqueNumbers()
    {
        var randomNumbers = RandomNumberGenerator.Sample(1000, 100);
        randomNumbers.Should().HaveCount(100).And.OnlyHaveUniqueItems();
    }
}