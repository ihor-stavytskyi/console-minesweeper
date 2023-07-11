// The class that contains the main logic of the game
public class Game : IGame
{
    /* 
    The board is represented as a 2D-array because this data structure is most similar to the real game board, 
    so it is intuitive for a human to imagine a board as a 2D-array. 
    Each element of this array contains an instance of a Cell class that represents the state of a single cell -  
    whether it is a hole or not, whether it was opened by the user or not, how many adjacent holes it has and so on.
    */
    private readonly Cell[,] _board;
    
    private readonly int _holesCount;

    // Represents shifts to get the neighbors. If you add these points to any point, you will get all its neighbors
    private readonly Point[] _shifts = new Point[] 
    {
        new Point(0, -1),   // ←
        new Point(-1, -1),  // ↖ 
        new Point(-1, 0),   // ↑
        new Point(-1, 1),   // ↗
        new Point(0, 1),    // →
        new Point(1, 1),    // ↘
        new Point(1, 0),    // ↓
        new Point(1, -1),   // ↙
    };

    public Game(int boardSize, Point[] holeLocations)
    {
        // initialize the board
        _holesCount = holeLocations.Length;
        BoardSize = boardSize;
        _board = new Cell[boardSize, boardSize];
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                _board[i, j] = new Cell();
            }
        }

        // mark holes on the board
        foreach (var hole in holeLocations)
        {
            var cell = GetCell(hole);
            cell.IsHole = true;
        }

        CalculateAdjacentHolesCountForAllCells();
    }

    public int BoardSize { get; }

    public int OpenCellsCount { get; private set; }

    public bool IsInsideBoard(Point point)
    {
        return point.Row >= 0 && point.Row < BoardSize && point.Column >=0 && point.Column < BoardSize;
    }

    public IReadOnlyCell GetReadOnlyCell(Point point)
    {
        return GetCell(point);
    }
    
    public GameState Click(int row, int column)
    {
        return Click(new Point(row, column));
    }

    public GameState Click(Point clickedPoint)
    {
        if (!IsInsideBoard(clickedPoint))
        {
            return GameState.GameContinues;
        }

        var clickedCell = GetCell(clickedPoint);

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
        if (!IsInsideBoard(clickedPoint))
        {
            return;
        }

        var clickedCell = GetCell(clickedPoint);
        if (clickedCell.IsOpen)
        {
            return;
        }

        clickedCell.IsFlagged = !clickedCell.IsFlagged; // flag or unflag
        
        // change adjacent flags count and IsBlue property for neighbors
        var flagsToAdd = clickedCell.IsFlagged ? 1 : -1;
        var neighbors = GetNeighbors(clickedPoint);
        foreach (var neighborPoint in neighbors)
        {
            var neighbor = GetCell(neighborPoint);
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
        var neighbors = GetNeighbors(clickedPoint);
        foreach (var neighborPoint in neighbors)
        {
            var neighbor = GetCell(neighborPoint);
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
        var clickedCell = GetCell(clickedPoint);
        if (OpenCellsCount == 0) // the user can not hit a hole on the first click, so move the hole to a free cell
        {
            MoveHoleToTheFirstFreeCell(clickedCell);
            MarkCellsAsOpen(clickedPoint);
            return GameState.GameContinues;
        }
        else
        {
            OpenCell(clickedCell);
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
            var cellToOpen = GetCell(currentPoint);

            if (!cellToOpen.IsOpen)
            {
                OpenCell(cellToOpen);
                if (cellToOpen.AdjacentHolesCount == 0) // if a cell has zero adjacent holes, then its surrounding cells should be open as well
                {
                    var neighbors = GetNeighbors(currentPoint);
                    foreach (var neighbor in neighbors)
                    {
                        queue.Enqueue(neighbor); // add every neighbor to the queue
                    }
                }
            }
        }
    }

    /* 
    In the original Minesweeper game, you can't hit a mine on the first click. The same behavior is implemented here.
    The method moves the hole to the first free cell starting from the top left corner. 
    If the cell is not free, then it moves to the right. If the whole row is occupied, it moves one row down and so on.
    Note that after that we need to recalculate the number of adjacent holes.
    */
    private void MoveHoleToTheFirstFreeCell(Cell clickedCell)
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                var cell = GetCell(i, j);
                if (!cell.IsHole)
                {
                    cell.IsHole = true;
                    clickedCell.IsHole = false;
                    CalculateAdjacentHolesCountForAllCells();
                    return;
                }
            }
        }
    } 

    private void CalculateAdjacentHolesCountForAllCells()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                var point = new Point(i, j);
                var cell = GetCell(point);
                if (!cell.IsHole) // for holes we do not need to calculate the number of adjacent holes
                {
                    cell.AdjacentHolesCount = CalculateAdjacentHolesCount(point);
                }
            }
        }
    }

    // The idea is to get all neighbors and count the number of holes among the neighbors
    private int CalculateAdjacentHolesCount(Point point)
    {
        var neighbors = GetNeighbors(point);
        return neighbors.Where(x => GetCell(x).IsHole).Count();
    }

    // The method takes all eight possible neighbor points and filters out the points that are outside the board (because the edge cells have less than eight neighbors)
    private IEnumerable<Point> GetNeighbors(Point point)
    {
        return _shifts
            .Select(shift => new Point(point.Row + shift.Row, point.Column + shift.Column))
            .Where(neighborPoint => IsInsideBoard(neighborPoint));
    }

    // The user wins the game when all cells except holes are open
    private bool HasUserWon()
    {
        return OpenCellsCount == (BoardSize * BoardSize - _holesCount);
    }

    private Cell GetCell(Point point)
    {
        return GetCell(point.Row, point.Column);
    }

    private Cell GetCell(int row, int column)
    {
        return _board[row, column];
    }

    private void OpenCell(Cell cell)
    {
        cell.IsOpen = true;
        OpenCellsCount++;
    }
}