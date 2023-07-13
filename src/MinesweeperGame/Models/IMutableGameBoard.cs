/*
I decided to split up the interfaces of the game board into mutable and read-only ones, 
so that the game engine (Game.cs) can mutate the board by making moves, 
but the presentation logic (BoardPrinter.cs) can just read the board state, but not make changes to it.
*/
public interface IMutableGameBoard : IReadOnlyGameBoard
{
    Cell GetCell(Point point);
    void OpenCell(Cell cell);
    IEnumerable<Point> GetNeighbors(Point point);
    void MoveMineToTheFirstFreeCell(Cell clickedCell);
}