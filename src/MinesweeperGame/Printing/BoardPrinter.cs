public class BoardPrinter
{
    private readonly PrinterOptions _options;
    private readonly IReadOnlyGameBoard _gameBoard;

    public BoardPrinter(PrinterOptions options, IReadOnlyGameBoard gameBoard)
    {
        _options = options;
        _gameBoard = gameBoard;
    }

    public void RevealBoard()
    {
        PrintBoard(CellExtensions.RevealCell, true);
    }

    public void PrintBoardForGamer()
    {
        PrintBoard(CellExtensions.CellToStringForGamer, false);
    }

    private void PrintBoard(Func<IReadOnlyCell, string> cellToString, bool forceOpen)
    {
        PrintOpenedCellsCount();
        PrintHeaderRow();

        for (int i = 0; i < _gameBoard.BoardSize; i++)
        {
            PrintIndexCell(i);

            for (int j = 0; j < _gameBoard.BoardSize; j++)
            {
                var cell = _gameBoard.GetReadOnlyCell(new Point(i, j));
                var cellColor = GetCellColor(cell, forceOpen);
                var cellContent = cellToString(cell);
                PrintCell(cellContent, cellColor);
            }

            PrintEndOfRow();
        }

        Console.WriteLine();
    }

    private void PrintOpenedCellsCount()
    {
        Console.Write("You have opened ");
        PrintWithColor(_gameBoard.OpenCellsCount.ToString(), ConsoleColor.Green);
        Console.Write(" cells out of ");
        PrintWithColor((_gameBoard.BoardSize * _gameBoard.BoardSize).ToString(), ConsoleColor.Green);
        Console.WriteLine();
    }

    private void PrintHeaderRow()
    {
        PrintCell(" ");

        for (int i = 0; i < _gameBoard.BoardSize; i++)
        {
            PrintIndexCell(i);
        }

        PrintEndOfRow();
        PrintHeaderRowSeparator();
    }

    private void PrintHeaderRowSeparator()
    {
        // tricky logic to calculate how many times we should repeat the '-' symbol to cover the full row
        var cellWidth = _options.CellWidth + 1;
        var rowWidth = cellWidth * _gameBoard.BoardSize + cellWidth + 1;
        Console.WriteLine(new string('-', rowWidth));
    }

    // This method is needed to print a row or a column index
    private void PrintIndexCell(int number)
    {
        PrintCell(number.ToString(), ConsoleColor.Blue);
    }

    private void PrintCell(string cellContent)
    {
        PrintCell(cellContent, _options.DefaultColor);
    }

    private void PrintEndOfRow()
    {
        Console.WriteLine(_options.VerticalCellSeparator);
    }

    private void PrintCell(string cellContent, ConsoleColor foregroundColor)
    {
        Console.Write(_options.VerticalCellSeparator);
        var alignedCellContent = cellContent.AlignToCenter(_options.CellWidth);
        PrintWithColor(alignedCellContent, foregroundColor);
    }

    private void PrintWithColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = _options.DefaultColor;
    }

    private ConsoleColor GetCellColor(IReadOnlyCell cell, bool forceOpen)
    {
        return cell.IsOpen || forceOpen
            ? cell.IsOpenAndClickable
                ? _options.OpenClickableCellColor
                : cell.IsMine 
                    ? _options.MineColor 
                    : _options.OpenCellColor 
            : cell.IsFlagged
                ? _options.FlaggedCellColor
                : _options.DefaultColor;
    }
}