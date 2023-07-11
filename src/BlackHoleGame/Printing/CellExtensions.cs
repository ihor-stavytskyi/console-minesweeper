public static class CellExtensions
{
    public static string RevealCell(this IReadOnlyCell cell)
    {
        return cell.IsHole 
            ? "H" // "H" for "Hole"
            : cell.AdjacentHolesCount == 0 
                ? " " 
                : cell.AdjacentHolesCount.ToString();
    }

    public static string CellToStringForGamer(this IReadOnlyCell cell)
    {
        return cell.IsOpen 
            ? RevealCell(cell) 
            : cell.IsFlagged 
                ? "F" // "F" for "Flagged"
                : "C"; // "C" for "Closed"
    }
}