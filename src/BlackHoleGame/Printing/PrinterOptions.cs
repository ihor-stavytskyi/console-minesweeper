public class PrinterOptions
{
    public ConsoleColor DefaultColor { get; set; } = ConsoleColor.Gray;
    public ConsoleColor OpenCellColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor HoleColor { get; set; } = ConsoleColor.Red;
    public char VerticalCellSeparator { get; set; } = '|';
    public int CellWidth { get; set; } = 3;
}