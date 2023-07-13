public interface IReadOnlyGameBoard
{
    int BoardSize { get; }
    int MinesCount { get; }
    int OpenCellsCount { get; }
    bool IsMineOpen { get; }
    bool IsInsideBoard(Point point);
    IReadOnlyCell GetReadOnlyCell(Point point);
}