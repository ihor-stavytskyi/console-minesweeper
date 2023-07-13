public static class CellExtensions
{
    public static string RevealCell(this IReadOnlyCell cell)
    {
        return cell.IsMine 
            ? "M" // "M" for "Mine"
            : cell.AdjacentMinesCount == 0 
                ? " " 
                : cell.AdjacentMinesCount.ToString();
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