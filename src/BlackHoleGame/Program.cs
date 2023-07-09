﻿var userInputOutput = new UserInputOutput(new GameOptions());
userInputOutput.PrintInstructions();
var boardSize = userInputOutput.ReadBoardSizeFromConsole();
var holesCount = userInputOutput.ReadHolesCountFromConsole(boardSize);

var randomNumbers = RandomNumberGenerator.Sample(boardSize * boardSize, holesCount);
var holeLocations = NumberToPointConverter.Convert(randomNumbers, boardSize);
var game = new Game(boardSize, holeLocations);
var printer = new BoardPrinter(new PrinterOptions(), game);
printer.PrintBoardForGamer();
var gameState = GameState.GameContinues;

do
{
    var pointToClick = UserInputOutput.ReadPointFromConsole(game);
    if (pointToClick == UserInputOutput.RevealBoardCommand) // that is a trick that reveals the board for testing purposes
    {
        printer.RevealBoard();
    }
    else
    {
        gameState = game.Click(pointToClick);
        if (gameState == GameState.GameContinues)
        {
            printer.PrintBoardForGamer();
        }
    }
} while (gameState == GameState.GameContinues);

printer.RevealBoard();
userInputOutput.PrintResult(gameState);