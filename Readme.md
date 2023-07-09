# Black Hole Game
This is a console version of the Black Hole game, which is an analog of the Minesweeper game. Please note it is a fully functional game except for the flagging functionality. You will find the instructions on how to play when you run the console application.

## How to run?
You can run this application either by having `.NET SDK 7.0` installed locally or using Docker.

### Docker
You should have Docker installed on your machine.
First, navigate to the folder with the application project:

```sh
cd src/BlackHoleGame
```

Then, build a Docker image:

```
docker build -t black-hole-game-image -f Dockerfile .
```

After the Docker image is built, you can run a Docker container:
```
docker run -it --rm black-hole-game-image
```

### .NET
To run the application locally without Docker, you should have `.NET SDK 7.0` installed. You can download it [here](https://dotnet.microsoft.com/en-us/download).

After you downloaded and installed `.NET SDK 7.0`, navigate to the folder with the application project:

```sh
cd src/BlackHoleGame
```

Then, build the project:
```
dotnet build
```

Then, run the application:
```
dotnet run
```

## Tests
I covered with unit tests everything except the user input/output and the game board printing functionality.
To run the tests, you should have `.NET SDK 7.0` installed. You can find it [here](https://dotnet.microsoft.com/en-us/download).
After you have `.NET SDK 7.0` installed, navigate to the folder with the unit tests:

```sh
cd src/BlackHoleGameTests
```

Then, build the unit test project:
```
dotnet build
```

Then, run the tests:
```
dotnet test
```

## Test coverage report
To see the test coverage report, navigate to the folder with the report:
```sh
cd src/test-coverage-report
```
Then, open the `index.html` in your browser.
