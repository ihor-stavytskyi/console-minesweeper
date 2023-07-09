using FluentAssertions;

namespace BlackHoleGameTests;

public class NumberToPointConverterTests
{
    [Theory]
    [InlineData(24, 2, 4)]
    [InlineData(5, 0, 5)]
    [InlineData(90, 9, 0)]
    public void ConvertShouldTransformNumberToPoint(int number, int expectedRow, int expectedColumn)
    {
        var point = NumberToPointConverter.Convert(new [] { number }, 10).Single();
        point.Row.Should().Be(expectedRow);
        point.Column.Should().Be(expectedColumn);
    }
}