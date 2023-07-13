// Represents the state of a single cell on the board
public class Cell : IReadOnlyCell
{
    public bool IsOpen { get; set; }
    public bool IsMine { get; set; }
    public bool IsFlagged { get; set; }
    public int AdjacentMinesCount { get; set; }
    public int AdjacentFlagsCount { get; set; }
    public bool IsOpenAndClickable => IsOpen && !IsMine && AdjacentFlagsCount == AdjacentMinesCount;
}