using FluentAssertions;

namespace BlackHoleGameTests;

public class GameTests
{
    // Verifies that adjacent holes counts are calculated and holes are marked on the board correctly
    [Fact]
    public void CellsSurroundingHoleShouldHaveAdjacentHolesCountEqualToOne()
    {
        var hole = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { hole });
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
                var expectedAdjacentHolesCount = adjacentPoints.Contains(point) ? 1 : 0;
                cell.AdjacentHolesCount.Should().Be(expectedAdjacentHolesCount);

                var expectedIsHole = point == hole;
                cell.IsHole.Should().Be(expectedIsHole);
            }
        }
    }

    // When the user clicks on a cell that has zero adjacent holes, then all surrounding cells should become open as well, and this is recursive
    [Fact]
    public void ClickOnZeroAdjacentHolesCellShouldOpenSurroundingCells()
    {
        var hole = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { hole });
        var game = new Game(gameBoard);

        var gameState = game.Click(0, 0);
        gameState.Should().Be(GameState.UserWon); // the user wins the game because all cells become open after the click

        for (int i = 0; i < gameBoard.BoardSize; i++)
        {
            for (int j = 0; j < gameBoard.BoardSize; j++)
            {
                var point = new Point(i, j);
                var cell = gameBoard.GetReadOnlyCell(point);
                var expectedIsOpen = point != hole; // every cell except the hole should become open
                cell.IsOpen.Should().Be(expectedIsOpen);
            }
        }
    }

    // When the user clicks on a cell that has more than zero adjacent holes, then just the clicked cell should become open
    [Fact]
    public void ClickOnMoreThanZeroAdjacentHolesCellShouldOpenJustClickedCell()
    {
        var hole = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { hole });
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

    // When the user hits a hole on a first click, the hole should be moved
    [Fact]
    public void FirstEverClickOnHoleShouldMoveTheHole()
    {
        // Scenario: user clicks on (2, 2) which is a hole. The game moves the hole to (1, 0). 
        // Explanation why the (1, 0) is chosen - because it is the first free cell starting from the top left corner (the whole first row is occupied by other holes)

        var holes = new [] 
        {
            new Point(0, 0),
            new Point(0, 1),
            new Point(0, 2),
            new Point(0, 3),
            new Point(0, 4),
            new Point(2, 2),
            new Point(2, 3),
        };
        var gameBoard = new GameBoard(5, holes);
        var game = new Game(gameBoard);

        // this cell should be free
        var cellBeforeFirstClick = gameBoard.GetReadOnlyCell(new Point(1, 0));
        cellBeforeFirstClick.IsHole.Should().BeFalse(); // cell is not a hole

        var gameStateAfterFirstClick = game.Click(2, 2); // hit the hole on a first click
        gameStateAfterFirstClick.Should().Be(GameState.GameContinues);

        // the hole should be moved to the first empty cell starting from the top left corner
        var cellAfterFirstClick = gameBoard.GetReadOnlyCell(new Point(1, 0));
        cellAfterFirstClick.IsHole.Should().BeTrue(); // now this cell is a hole

        var gameStateAfterSecondClick = game.Click(0, 1);
        gameStateAfterSecondClick.Should().Be(GameState.UserLost); // on a second click user hits a hole and loses the game
    }

    // When the user clicks on a hole, the game is over
    [Fact]
    public void ClickOnHoleShouldLoseTheGame()
    {
        var hole = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { hole });
        var game = new Game(gameBoard);
        var gameStateAfterFirstClick = game.Click(1, 1); // we do not hit a hole on the first click, because it impossible to lose on the first click
        gameStateAfterFirstClick.Should().Be(GameState.GameContinues);

        var gameStateAfterSecondClick = game.Click(hole); // make the second click on the hole
        gameStateAfterSecondClick.Should().Be(GameState.UserLost); // because user hit a hole
    }

    // When the user tries to click outside the board, nothing bad should happen, the game continues
    [Fact]
    public void ClickOutsideTheBoardShouldNotBreakTheGame()
    {
        var hole = new Point(2, 2);
        var gameBoard = new GameBoard(5, new [] { hole });
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
        var hole = new Point(400, 400);
        var gameBoard = new GameBoard(1000, new [] { hole });
        var game = new Game(gameBoard);

        var gameState = game.Click(500, 500);
        gameState.Should().Be(GameState.UserWon); // user wins on the first click

        for (int i = 0; i < gameBoard.BoardSize; i++)
        {
            for (int j = 0; j < gameBoard.BoardSize; j++)
            {
                var point = new Point(i, j);
                var cell = gameBoard.GetReadOnlyCell(point);
                var expectedIsOpen = point != hole; // every cell except the hole should become open
                cell.IsOpen.Should().Be(expectedIsOpen);
            }
        }
    }
}