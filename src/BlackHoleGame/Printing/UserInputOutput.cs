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
        string message = "Welcome to the Black Hole game, the analog of the Minesweeper game. " +
            "You have a board of N*N cells and K black holes randomly placed on the board. " + 
            "Your goal is to open all the cells that are not holes. If you hit a hole, the game is over.\n" +
            "The coordinates of cells start from 0 and end with N-1. The top left corner has coordinates (0, 0) and the bottom right corner is (N-1, N-1).\n" +
            "Before the game is started, you have to enter the numbers N and K. Then, on each move, you should enter the coordinates of the cell you want to open separated by a space.\n" + 
            "How to understand the board?\n" +
            "'C' is a closed cell.\n" +
            "'H' is a hole.\n" + 
            "'#' is the number of adjacent holes. For example, if you see '2', this cell contains two holes in the surrounding cells.\n\n" +
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

    public int ReadHolesCountFromConsole(int boardSize)
    {
        var holesCount = 0;
        while (true)
        {
            holesCount = ReadNumberFromConsole("Please enter the number of the black holes on the board: ");
            if (holesCount < 1)
            {
                PrintError("At least one hole should be placed on the board.");
            }
            else if (holesCount > boardSize * boardSize - _options.MinFreeCells)
            {
                PrintError($"You should have at least {_options.MinFreeCells} free cells.");
            }
            else
            {
                break;
            }
        }

        Console.WriteLine();

        return holesCount;
    }

    public static (UserCommand Command, Point Point) ReadPointFromConsole(IGame game)
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
                if (!game.IsInsideBoard(point))
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
            : ("Unfortunately you have hit a black hole.", ConsoleColor.Red);
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