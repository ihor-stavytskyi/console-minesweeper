using FluentAssertions;

namespace BlackHoleGameTests;

public class CellExtensionsTests
{
    [Fact]
    public void CellToStringForGamerShouldHandleHole()
    {
        var cell = new Cell()
        {
            IsOpen = true,
            IsHole = true,
            AdjacentHolesCount = 0
        };
        var stringRepresentation = cell.CellToStringForGamer();
        stringRepresentation.Should().Be("H");
    }

    [Fact]
    public void CellToStringForGamerShouldHandleEmptyCellAndReturnAdjacentHolesCount()
    {
        var cell = new Cell()
        {
            IsOpen = true,
            IsHole = false,
            AdjacentHolesCount = 2
        };
        var stringRepresentation = cell.CellToStringForGamer();
        stringRepresentation.Should().Be("2");
    }

    [Fact]
    public void CellToStringForGamerShouldHandleClosedCell()
    {
        var cell = new Cell()
        {
            IsOpen = false,
            IsHole = false,
            AdjacentHolesCount = 2
        };
        var stringRepresentation = cell.CellToStringForGamer();
        stringRepresentation.Should().Be("C");
    }

    [Fact]
    public void RevealCellShouldShowTheHoleDespiteTheCellIsClosed()
    {
        var cell = new Cell()
        {
            IsOpen = false,
            IsHole = true,
            AdjacentHolesCount = 0
        };
        var stringRepresentation = cell.RevealCell();
        stringRepresentation.Should().Be("H");
    }

    [Fact]
    public void RevealCellShouldShowAdjacentHolesCountDespiteTheCellIsClosed()
    {
        var cell = new Cell()
        {
            IsOpen = false,
            IsHole = false,
            AdjacentHolesCount = 3
        };
        var stringRepresentation = cell.RevealCell();
        stringRepresentation.Should().Be("3");
    }
}