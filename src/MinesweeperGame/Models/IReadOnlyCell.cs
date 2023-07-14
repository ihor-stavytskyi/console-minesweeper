// The interface represents a read-only cell. It is needed to prevent BoardPrinter from modifying the state of a cell.
public interface IReadOnlyCell
{
    bool IsOpen { get; }
    bool IsMine { get; }
    public bool IsFlagged { get; }
    int AdjacentMinesCount { get; }
    int AdjacentFlagsCount { get; }
    public bool IsOpenAndClickable { get; }
}
