// The interface represents a read-only cell. It is needed to prevent BoardPrinter from modifying the state of a cell.
public interface IReadOnlyCell
{
    bool IsOpen { get; }
    bool IsHole { get; }
    int AdjacentHolesCount { get; }
}
