// The class that contains the main logic of the game
public class GameBoard : IMutableGameBoard
{
    /* 
    The board is represented as a 2D-array because this data structure is most similar to the real game board, 
    so it is intuitive for a human to imagine a board as a 2D-array. 
    Each element of this array contains an instance of a Cell class that represents the state of a single cell -  
    whether it is a mine or not, whether it was opened by the user or not, how many adjacent mines it has and so on.
    */
    private readonly Cell[,] _board;

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

    public GameBoard(int boardSize, Point[] mineLocations)
    {
        // initialize the board
        MinesCount = mineLocations.Length;
        BoardSize = boardSize;
        _board = new Cell[boardSize, boardSize];
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                _board[i, j] = new Cell();
            }
        }

        // mark mines on the board
        foreach (var mine in mineLocations)
        {
            var cell = GetCell(mine);
            cell.IsMine = true;
        }

        CalculateAdjacentMinesCountForAllCells();
    }

    public int BoardSize { get; }

    public int MinesCount { get; }

    public int TotalCellsToOpen => BoardSize * BoardSize - MinesCount;

    public int OpenCellsCount { get; private set; }

    public bool IsMineOpen { get; private set; }

    public bool IsInsideBoard(Point point)
    {
        return point.Row >= 0 && point.Row < BoardSize && point.Column >=0 && point.Column < BoardSize;
    }

    public IReadOnlyCell GetReadOnlyCell(Point point)
    {
        return GetCell(point);
    }

    public Cell GetCell(Point point)
    {
        return GetCell(point.Row, point.Column);
    }
    
    public void OpenCell(Cell cell)
    {
        cell.IsOpen = true;
        OpenCellsCount++;
        if (cell.IsMine)
        {
            IsMineOpen = true;
        }
    }

    /* 
    In the original Minesweeper game, you can't hit a mine on the first click. The same behavior is implemented here.
    The method moves the mine to the first free cell starting from the top left corner. 
    If the cell is not free, then it moves to the right. If the whole row is occupied, it moves one row down and so on.
    Note that after that we need to recalculate the number of adjacent mines.
    */
    public void MoveMineToTheFirstFreeCell(Cell clickedCell)
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                var cell = GetCell(i, j);
                if (!cell.IsMine)
                {
                    cell.IsMine = true;
                    clickedCell.IsMine = false;
                    CalculateAdjacentMinesCountForAllCells();
                    return;
                }
            }
        }
    } 

    // The method takes all eight possible neighbor points and filters out the points that are outside the board (because the edge cells have less than eight neighbors)
    public IEnumerable<Point> GetNeighbors(Point point)
    {
        return _shifts
            .Select(shift => new Point(point.Row + shift.Row, point.Column + shift.Column))
            .Where(neighborPoint => IsInsideBoard(neighborPoint));
    }

    private Cell GetCell(int row, int column)
    {
        return _board[row, column];
    }

    private void CalculateAdjacentMinesCountForAllCells()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                var point = new Point(i, j);
                var cell = GetCell(point);
                if (!cell.IsMine) // for mines we do not need to calculate the number of adjacent mines
                {
                    cell.AdjacentMinesCount = CalculateAdjacentMinesCount(point);
                }
            }
        }
    }

    // The idea is to get all neighbors and count the number of mines among the neighbors
    private int CalculateAdjacentMinesCount(Point point)
    {
        var neighbors = GetNeighbors(point);
        return neighbors.Where(x => GetCell(x).IsMine).Count();
    }
}