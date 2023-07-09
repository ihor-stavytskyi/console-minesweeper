// Represents the state of a single cell on the board
public class Cell : IReadOnlyCell
{
    public bool IsOpen { get; set; }
    public bool IsHole { get; set; }
    public int AdjacentHolesCount { get; set; }
}