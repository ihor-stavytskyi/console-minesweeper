// The class that contains the main logic of the game
public class Game
{
    private readonly IMutableGameBoard _board;

    public Game(IMutableGameBoard board)
    {
        _board = board;
    }
    
    public GameState Click(int row, int column)
    {
        return Click(new Point(row, column));
    }

    public GameState Click(Point clickedPoint)
    {
        if (!_board.IsInsideBoard(clickedPoint))
        {
            return GameState.GameContinues;
        }

        var clickedCell = _board.GetCell(clickedPoint);

        if (clickedCell.IsFlagged)
        {
            return GameState.GameContinues;
        }

        if (clickedCell.IsHole)
        {
            return HandleClickIntoHole(clickedPoint);
        }

        else if (clickedCell.IsOpenAndClickable)
        {
            return HandleClickIntoOpenAndClickableCell(clickedPoint);
        }
        
        MarkCellsAsOpen(clickedPoint);

        return HasUserWon() ? GameState.UserWon : GameState.GameContinues;
    }

    public void Flag(Point clickedPoint)
    {
        if (!_board.IsInsideBoard(clickedPoint))
        {
            return;
        }

        var clickedCell = _board.GetCell(clickedPoint);
        if (clickedCell.IsOpen)
        {
            return;
        }

        clickedCell.IsFlagged = !clickedCell.IsFlagged; // flag or unflag
        
        // change adjacent flags count and IsBlue property for neighbors
        var flagsToAdd = clickedCell.IsFlagged ? 1 : -1;
        var neighbors = _board.GetNeighbors(clickedPoint);
        foreach (var neighborPoint in neighbors)
        {
            var neighbor = _board.GetCell(neighborPoint);
            neighbor.AdjacentFlagsCount += flagsToAdd; 
        }

        /* Requirements for flagging:

        [DONE] Modifications to Cell:
        1. Introduce property AreAllHolesFlagged. This property should be set to true only if the cell itself is open 
        and the number of flags in adjacent cells is greather than or equal to the AdjacentHolesCount.

        [DONE] Actions for Flag():
        1. If clicked cell is already open, then nothing happens
        2. If clicked cell is closed, then it should be marked with flag.
        3. If clicked cell is flagged, it should be unflagged.
        4. When a new flag is set or removed, it should affect AreAllHolesFlagged of surrounding cells to the clicked one.

        Modifications to Click():
        1. If user clicked into a cell with AreAllHolesFlagged = true, 
        the algorithm should open all the surrounding cells except flagged ones. 
        BUT if user flagged a wrong cell (that does not contain a hole), user should lose the game immediately.
              
        [DONE] Modifications to presentation:
        1. If a cell is flagged, it should display F
        2. If AreAllHolesFlagged is true, cell should become BLUE.
        */
    }

    private GameState HandleClickIntoOpenAndClickableCell(Point clickedPoint)
    {
        var neighbors = _board.GetNeighbors(clickedPoint);
        foreach (var neighborPoint in neighbors)
        {
            var neighbor = _board.GetCell(neighborPoint);
            if (!neighbor.IsOpen)
            {
                if (!neighbor.IsFlagged)
                {
                    MarkCellsAsOpen(neighborPoint);
                }
                else if (!neighbor.IsHole) // if user flagged empty cell
                {
                    return GameState.UserLost;
                }
            }
        }

        return GameState.GameContinues;
    }

    private GameState HandleClickIntoHole(Point clickedPoint)
    {
        var clickedCell = _board.GetCell(clickedPoint);
        if (_board.OpenCellsCount == 0) // the user can not hit a hole on the first click, so move the hole to a free cell
        {
            _board.MoveHoleToTheFirstFreeCell(clickedCell);
            MarkCellsAsOpen(clickedPoint);
            return GameState.GameContinues;
        }
        else
        {
            _board.OpenCell(clickedCell);
            return GameState.UserLost;
        }
    }

    /* 
    If a cell has zero adjacent holes, then the surrounding cells should be open as well. So it is a recursive procedure. 
    
    I decided to make a test. I implemented the method with both options (recursion and using a queue), then wrote a unit test 
    that creates a huge board of size 1000*1000 cells with a single hole, so the first click recursively opens (1000*1000 - 1) cells. 
    The recursive method threw a Stack Overflow exception, which was expected, and the method with a queue did very well 
    and did not crash the application. Though for the small boards both methods work well, I decided to move on with the method that uses a queue. 
    The unit test is also available (see GameTests.EnormousBoardShouldNotCrashTheGame).
    */

    private void MarkCellsAsOpen(Point clickedPoint)
    {
        var queue = new Queue<Point>();
        queue.Enqueue(clickedPoint);
        while (queue.Any())
        {
            var currentPoint = queue.Dequeue();
            var cellToOpen = _board.GetCell(currentPoint);

            if (!cellToOpen.IsOpen)
            {
                _board.OpenCell(cellToOpen);
                if (cellToOpen.AdjacentHolesCount == 0) // if a cell has zero adjacent holes, then its surrounding cells should be open as well
                {
                    var neighbors = _board.GetNeighbors(currentPoint);
                    foreach (var neighbor in neighbors)
                    {
                        queue.Enqueue(neighbor); // add every neighbor to the queue
                    }
                }
            }
        }
    }

    // The user wins the game when all cells except holes are open
    private bool HasUserWon()
    {
        return _board.OpenCellsCount == (_board.BoardSize * _board.BoardSize - _board.HolesCount);
    }
}