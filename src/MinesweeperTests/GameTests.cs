using FluentAssertions;

namespace MinesweeperGameTests;

public class GameTests
{
    // Verifies that adjacent mines counts are calculated and mines are marked on the board correctly
    [Fact]
    public void CellsSurroundingMineShouldHaveAdjacentMinesCountEqualToOne()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        var adjacentPoints = new []
        {
            new Point(2, 1),
            new Point(1, 1),
            new Point(1, 2),
            new Point(1, 3),
            new Point(2, 3),
            new Point(3, 3),
            new Point(3, 2),
            new Point(3, 1),
        };
        
        for (int i = 0; i < gameBoard.BoardSize; i++)
        {
            for (int j = 0; j < gameBoard.BoardSize; j++)
            {
                var point = new Point(i, j);
                var cell = gameBoard.GetReadOnlyCell(point);
                var expectedAdjacentMinesCount = adjacentPoints.Contains(point) ? 1 : 0;
                cell.AdjacentMinesCount.Should().Be(expectedAdjacentMinesCount);

                var expectedIsMine = point == mine;
                cell.IsMine.Should().Be(expectedIsMine);
            }
        }
    }

    // When the user clicks on a cell that has zero adjacent mines, then all surrounding cells should become open as well, and this is recursive
    [Fact]
    public void ClickOnZeroAdjacentMinesCellShouldOpenSurroundingCells()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);

        var gameState = game.Click(0, 0);
        gameState.Should().Be(GameState.UserWon); // the user wins the game because all cells become open after the click

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
    }

    // When the user clicks on a cell that has more than zero adjacent mines, then just the clicked cell should become open
    [Fact]
    public void ClickOnMoreThanZeroAdjacentMinesCellShouldOpenJustClickedCell()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        var clickedPoint = new Point(1, 1);
        var gameState = game.Click(clickedPoint);
        gameState.Should().Be(GameState.GameContinues); // because not all cells are open

        for (int i = 0; i < gameBoard.BoardSize; i++)
        {
            for (int j = 0; j < gameBoard.BoardSize; j++)
            {
                var point = new Point(i, j);
                var cell = gameBoard.GetReadOnlyCell(point);
                var expectedIsOpen = point == clickedPoint;
                cell.IsOpen.Should().Be(expectedIsOpen);
            }
        }
    }

    // When the user hits a mine on a first click, the mine should be moved
    [Fact]
    public void FirstEverClickOnMineShouldMoveTheMine()
    {
        /*
        Scenario: user clicks on (2, 2) which is a mine. The game moves the mine to (1, 0). 
        Explanation: the (1, 0) point is chosen because it is the first free cell starting from the top left corner, 
        since the whole first row is occupied by other mines.
        */

        var mines = new [] 
        {
            new Point(0, 0),
            new Point(0, 1),
            new Point(0, 2),
            new Point(0, 3),
            new Point(0, 4),
            new Point(2, 2),
            new Point(2, 3),
        };
        var gameBoard = new GameBoard(5, mines);
        var game = new Game(gameBoard);

        // this cell should be free
        var cellBeforeFirstClick = gameBoard.GetReadOnlyCell(new Point(1, 0));
        cellBeforeFirstClick.IsMine.Should().BeFalse(); // cell is not a mine

        var gameStateAfterFirstClick = game.Click(2, 2); // hit the mine on a first click
        gameStateAfterFirstClick.Should().Be(GameState.GameContinues);

        // the mine should be moved to the first empty cell starting from the top left corner
        var cellAfterFirstClick = gameBoard.GetReadOnlyCell(new Point(1, 0));
        cellAfterFirstClick.IsMine.Should().BeTrue(); // now this cell is a mine

        var gameStateAfterSecondClick = game.Click(0, 1);
        gameStateAfterSecondClick.Should().Be(GameState.UserLost); // on a second click user hits a mine and loses the game
    }

    // When the user clicks on a mine, the game is over
    [Fact]
    public void ClickOnMineShouldLoseTheGame()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);
        var gameStateAfterFirstClick = game.Click(1, 1); // we do not hit a mine on the first click, because it impossible to lose on the first click
        gameStateAfterFirstClick.Should().Be(GameState.GameContinues);

        var gameStateAfterSecondClick = game.Click(mine); // make the second click on the mine
        gameStateAfterSecondClick.Should().Be(GameState.UserLost); // because user hit a mine
    }

    // When the user tries to click outside the board, nothing bad should happen, the game continues
    [Fact]
    public void ClickOutsideTheBoardShouldNotBreakTheGame()
    {
        var mine = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { mine });
        var game = new Game(gameBoard);

        var gameState = game.Click(8, 8); // click outside the board
        gameState.Should().Be(GameState.GameContinues);

        gameState = game.Click(-8, -8); // another click outside the board
        gameState.Should().Be(GameState.GameContinues);
    }

    // Generate a huge board and make a click that opens almost all the cells to verify the game can handle such a big board size
    [Fact]
    public void EnormousBoardShouldNotCrashTheGame()
    {
        var mine = new Point(400, 400);
        var gameBoard = new GameBoard(1000, new [] { mine });
        var game = new Game(gameBoard);

        var gameState = game.Click(500, 500);
        gameState.Should().Be(GameState.UserWon); // user wins on the first click

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
    }
}