
using FluentAssertions;

namespace MinesweeperGameTests;

public class FlaggingTests
{
    [Fact]
    public void ClickIntoFlaggedCellDoesNothing()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        
        game.Flag(mine); // flag the mine
        var gameStateAfterFirstClick = game.Click(mine); // click on the mine
        gameStateAfterFirstClick.Should().Be(GameState.GameContinues);
    }

    [Fact]
    public void AttemptToSetFlagOnOpenCellDoesNothing()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        
        var gameStateAfterFirstClick = game.Click(new Point(2, 1));
        game.Flag(new Point(2, 1));
        var cell = gameBoard.GetReadOnlyCell(new Point(2, 1));
        cell.IsFlagged.Should().BeFalse();
    }

    [Fact]
    public void FlaggingShouldAffectAdjacentFlagsCountOfSurroundingCells()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        
        game.Flag(new Point(2, 2));

        var cellsToCheck = new [] 
        {
            new Point(1, 1),
            new Point(1, 2),
            new Point(1, 3),
            new Point(2, 1),
            new Point(2, 3),
            new Point(3, 1),
            new Point(3, 2),
            new Point(3, 3),
        };

        foreach (var point in cellsToCheck)
        {
            var cell = gameBoard.GetReadOnlyCell(point);
            cell.AdjacentFlagsCount.Should().Be(1);   
        }
    }

    [Fact]
    public void UnflaggingShouldAffectAdjacentFlagsCountOfSurroundingCells()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        
        game.Flag(new Point(2, 2)); // flag
        game.Flag(new Point(2, 2)); // unflag

        var cellsToCheck = new [] 
        {
            new Point(1, 1),
            new Point(1, 2),
            new Point(1, 3),
            new Point(2, 1),
            new Point(2, 3),
            new Point(3, 1),
            new Point(3, 2),
            new Point(3, 3),
        };

        foreach (var point in cellsToCheck)
        {
            var cell = gameBoard.GetReadOnlyCell(point);
            cell.AdjacentFlagsCount.Should().Be(0);   
        }
    }

    [Fact]
    public void HandleClickIntoOpenAndClickableCellShouldWork()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        
        var gameStateAfterFirstClick = game.Click(new Point(2, 1));
        game.Flag(new Point(2, 2));
        var gameStateAfterSecondClick = game.Click(new Point(2, 1));

        for (int i = 0; i < gameBoard.BoardSize; i++)
        {
            for (int j = 0; j < gameBoard.BoardSize; j++)
            {
                var point = new Point(i, j);
                var cell = gameBoard.GetReadOnlyCell(point);
                var expectedIsOpen = point != mine; // every cell except the mine should become open
                cell.IsOpen.Should().Be(expectedIsOpen);
            }
        }

        gameStateAfterSecondClick.Should().Be(GameState.UserWon);
    }

    [Fact]
    public void FlagginWrongCellShouldLoseTheGame()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        var cellToFlag = new Point(1, 1);

        var gameStateAfterFirstClick = game.Click(new Point(2, 1));
        game.Flag(cellToFlag);
        var gameStateAfterSecondClick = game.Click(new Point(2, 1));

        for (int i = 0; i < gameBoard.BoardSize; i++)
        {
            for (int j = 0; j < gameBoard.BoardSize; j++)
            {
                var point = new Point(i, j);
                var cell = gameBoard.GetReadOnlyCell(point);
                var expectedIsOpen = point != cellToFlag; // every cell except the flagged one should be open
                cell.IsOpen.Should().Be(expectedIsOpen, because: $"{cell}");
            }
        }
        
        gameBoard.IsMineOpen.Should().BeTrue();
        gameStateAfterSecondClick.Should().Be(GameState.UserLost);
    }

    // When the user tries to set flag outside the board, nothing bad should happen, the game continues
    [Fact]
    public void FlagOutsideTheBoardShouldNotBreakTheGame()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);

        game.Flag(new Point(8, 8)); // try to set flag outside the board
        game.Flag(new Point(-8, -8)); // another try to set flag outside the board
        var gameState = game.Click(0, 0);
        gameState.Should().Be(GameState.UserWon);
    }
}