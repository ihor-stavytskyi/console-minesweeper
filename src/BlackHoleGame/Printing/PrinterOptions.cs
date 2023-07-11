public class PrinterOptions
{
    public ConsoleColor DefaultColor { get; set; } = ConsoleColor.Gray;
    public ConsoleColor OpenCellColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor HoleColor { get; set; } = ConsoleColor.Red;
    public ConsoleColor OpenClickableCellColor { get; set; } = ConsoleColor.Blue;
    public ConsoleColor FlaggedCellColor { get; set; } = ConsoleColor.Cyan;
    public char VerticalCellSeparator { get; set; } = '|';
    public int CellWidth { get; set; } = 3;
}