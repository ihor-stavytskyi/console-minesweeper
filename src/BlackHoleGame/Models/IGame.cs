// The interface is needed for the user input/output functionality and for the board printing.
public interface IGame
{
    int BoardSize { get; }
    int OpenCellsCount { get; }
    bool IsInsideBoard(Point point);
    IReadOnlyCell GetReadOnlyCell(Point point);
}