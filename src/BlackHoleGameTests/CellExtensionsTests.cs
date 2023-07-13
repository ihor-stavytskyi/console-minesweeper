using FluentAssertions;

namespace BlackHoleGameTests;

public class CellExtensionsTests
{
    [Fact]
    public void CellToStringForGamerShouldHandleMine()
    {
        var cell = new Cell()
        {
            IsOpen = true,
            IsMine = true,
            AdjacentMinesCount = 0
        };
        var stringRepresentation = cell.CellToStringForGamer();
        stringRepresentation.Should().Be("M");
    }

    [Fact]
    public void CellToStringForGamerShouldHandleEmptyCellAndReturnAdjacentMinesCount()
    {
        var cell = new Cell()
        {
            IsOpen = true,
            IsMine = false,
            AdjacentMinesCount = 2
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
            IsMine = false,
            AdjacentMinesCount = 2
        };
        var stringRepresentation = cell.CellToStringForGamer();
        stringRepresentation.Should().Be("C");
    }

    [Fact]
    public void CellToStringForGamerShouldHandleFlaggedCell()
    {
        var cell = new Cell()
        {
            IsOpen = false,
            IsMine = true,
            IsFlagged = true
        };
        var stringRepresentation = cell.CellToStringForGamer();
        stringRepresentation.Should().Be("F");
    }

    [Fact]
    public void RevealCellShouldShowTheMineDespiteTheCellIsClosed()
    {
        var cell = new Cell()
        {
            IsOpen = false,
            IsMine = true,
            AdjacentMinesCount = 0
        };
        var stringRepresentation = cell.RevealCell();
        stringRepresentation.Should().Be("M");
    }

    [Fact]
    public void RevealCellShouldShowAdjacentMinesCountDespiteTheCellIsClosed()
    {
        var cell = new Cell()
        {
            IsOpen = false,
            IsMine = false,
            AdjacentMinesCount = 3
        };
        var stringRepresentation = cell.RevealCell();
        stringRepresentation.Should().Be("3");
    }
}