﻿var userInputOutput = new UserInputOutput(new GameOptions());
userInputOutput.PrintInstructions();
var boardSize = userInputOutput.ReadBoardSizeFromConsole();
var minesCount = userInputOutput.ReadMinesCountFromConsole(boardSize);

var randomNumbers = RandomNumberGenerator.Sample(boardSize * boardSize, minesCount);
var mineLocations = NumberToPointConverter.Convert(randomNumbers, boardSize);
var gameBoard = new GameBoard(boardSize, mineLocations);
var game = new Game(gameBoard);
var printer = new BoardPrinter(new PrinterOptions(), gameBoard);
printer.PrintBoardForGamer();
var gameState = GameState.GameContinues;

do
{
    var (command, pointToClick) = UserInputOutput.ReadPointFromConsole(gameBoard);
    switch (command)
    {
        case UserCommand.RevealBoard: // this is a trick that reveals the board for testing purposes
            printer.RevealBoard();
            break;
        case UserCommand.Click:
            gameState = game.Click(pointToClick);
            break;
        case UserCommand.Flag:
            game.Flag(pointToClick);
            break;
    }

    if (gameState == GameState.GameContinues)
    {
        printer.PrintBoardForGamer();
    }
} while (gameState == GameState.GameContinues);

printer.RevealBoard();
userInputOutput.PrintResult(gameState);