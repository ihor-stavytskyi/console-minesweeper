public interface IReadOnlyGameBoard
{
    int BoardSize { get; }
    int HolesCount { get; }
    int OpenCellsCount { get; }
    bool IsInsideBoard(Point point);
    IReadOnlyCell GetReadOnlyCell(Point point);
}