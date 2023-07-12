// Represents the state of a single cell on the board
public class Cell : IReadOnlyCell
{
    public bool IsOpen { get; set; }
    public bool IsHole { get; set; }
    public bool IsFlagged { get; set; }
    public int AdjacentHolesCount { get; set; }
    public int AdjacentFlagsCount { get; set; }
    public bool IsOpenAndClickable => IsOpen && !IsHole && AdjacentFlagsCount == AdjacentHolesCount;
}