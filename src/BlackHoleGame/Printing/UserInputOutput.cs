public class UserInputOutput
{
    private readonly GameOptions _options;

    // The special hard-coded point that is handled as a command to reveal the board
    public static readonly Point RevealBoardCommand = new Point(-1, -1);

    public UserInputOutput(GameOptions options)
    {
        _options = options;
    }

    public void PrintInstructions()
    {
        string message = "Welcome to the Console Minesweeper game. " +
            "You have a board of N*N cells and K mines randomly placed on the board. " + 
            "Your goal is to open all the cells that are not mines. If you hit a mine, the game is over.\n" +
            "The coordinates of cells start from 0 and end with N-1. The top left corner has coordinates (0, 0) and the bottom right corner is (N-1, N-1).\n" +
            "Before the game is started, you have to enter the numbers N and K. " + 
            "Then, on each move, you should enter the coordinates of the cell you want to open separated by a space. For example: '3 4'. \n" + 
            "To flag or unflag a cell, add 'f' after the coordinates. For example: '3 4 f'." +
            "How to understand the board?\n" +
            "'C' is a closed cell.\n" +
            "'M' is a mine.\n" + 
            "'F' is a flag.\n" + 
            "'#' is the number of adjacent mines. For example, if you see '2', this cell contains two mines in the surrounding cells.\n\n" +
            "For testing purposes, you can reveal the board on any move by entering 'r' instead of the cell coordinates.\n" + 
            "Enjoy playing the game and good luck!\n";
        Console.WriteLine(message);
    }

    public int ReadBoardSizeFromConsole()
    {
        var boardSize = 0;
        while (true)
        {
            boardSize = ReadNumberFromConsole("You have a board of N*N cells. Please enter the number of N: ");
            if (boardSize < _options.MinBoardSize)
            {
                PrintError($"The board is too small. You can't have a board less than {_options.MinBoardSize}*{_options.MinBoardSize} cells.");
            }
            else if (boardSize > _options.MaxBoardSize)
            {
                PrintError($"The board is too big. You can't have a board greater than {_options.MaxBoardSize}*{_options.MaxBoardSize} cells.");
            }
            else
            {
                break;
            }
        }

        Console.WriteLine();

        return boardSize;
    }

    public int ReadMinesCountFromConsole(int boardSize)
    {
        var minesCount = 0;
        while (true)
        {
            minesCount = ReadNumberFromConsole("Please enter the number of the mines on the board: ");
            if (minesCount < 1)
            {
                PrintError("At least one mine should be placed on the board.");
            }
            else if (minesCount > boardSize * boardSize - _options.MinFreeCells)
            {
                PrintError($"You should have at least {_options.MinFreeCells} free cells.");
            }
            else
            {
                break;
            }
        }

        Console.WriteLine();

        return minesCount;
    }

    public static (UserCommand Command, Point Point) ReadPointFromConsole(IReadOnlyGameBoard gameBoard)
    {
        (UserCommand, Point) result;
        while (true)
        {
            Console.WriteLine("Enter the location of the cell you want to click on. Coordinates should be separated by a space:");
            var rawInput = Console.ReadLine()!;
            if (rawInput == "r") // "r" for "reveal"
            {
                result = (UserCommand.RevealBoard, RevealBoardCommand);
                break;
            }
            var values = rawInput.Trim().Split(' ');
            if ((values.Length == 2 || values.Length == 3) && TryParseNumber(values[0], out var row) && TryParseNumber(values[1], out var column))
            {
                var point = new Point(row, column);
                if (!gameBoard.IsInsideBoard(point))
                {
                    PrintError("The point must be inside the board.");
                }
                else
                {
                    if (values.Length == 2)
                    {
                        result = (UserCommand.Click, point);
                        break;
                    }
                    else // values.Length == 3
                    {
                        if (values[2] == "f")
                        {
                            result = (UserCommand.Flag, point);
                            break;
                        }
                    }
                }
            }
        }

        return result;
    }

    public void PrintResult(GameState state)
    {
        var (message, color) = state == GameState.UserWon
            ? ("Congratulations! You have won the game.", ConsoleColor.Green)
            : ("Unfortunately you have hit a mine.", ConsoleColor.Red);
        PrintWithColor(message, color);
    }

    private static int ReadNumberFromConsole(string message)
    {
        var parsedNumber = 0;
        while (true)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine()!;
            if (TryParseNumber(input, out parsedNumber))
            {
                break;
            }
        }

        return parsedNumber;
    }

    private static bool TryParseNumber(string enteredValue, out int number)
    {
        var parsed = int.TryParse(enteredValue, out number);
        if (!parsed)
        {
            PrintError("The entered symbols must be valid numbers.");
        }

        return parsed;
    }

    private static void PrintError(string message)
    {
        PrintWithColor(message, ConsoleColor.Red);
    }

    private static void PrintWithColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.Gray; // set the foreground color back to the standard one
    }
}