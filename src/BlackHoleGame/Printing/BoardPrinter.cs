public class BoardPrinter
{
    private readonly PrinterOptions _options;
    private readonly IGame _game;

    public BoardPrinter(PrinterOptions options, IGame game)
    {
        _options = options;
        _game = game;
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

        for (int i = 0; i < _game.BoardSize; i++)
        {
            PrintIndexCell(i);

            for (int j = 0; j < _game.BoardSize; j++)
            {
                var cell = _game.GetReadOnlyCell(new Point(i, j));
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
        PrintWithColor(_game.OpenCellsCount.ToString(), ConsoleColor.Green);
        Console.Write(" cells out of ");
        PrintWithColor((_game.BoardSize * _game.BoardSize).ToString(), ConsoleColor.Green);
        Console.WriteLine();
    }

    private void PrintHeaderRow()
    {
        PrintCell(" ");

        for (int i = 0; i < _game.BoardSize; i++)
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
        var rowWidth = cellWidth * _game.BoardSize + cellWidth + 1;
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
            ? cell.IsHole ? _options.HoleColor : _options.OpenCellColor 
            : _options.DefaultColor;
    }
}