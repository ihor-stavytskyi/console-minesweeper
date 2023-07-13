public static class NumberToPointConverter
{
    // Converts a number to a point. For example, if boardSize = 10, then number = 53 will be converted to a point (5, 3)
    public static Point[] Convert(int[] numbers, int boardSize)
    {
        return numbers
            .Select(number => 
            {
                var row = number / boardSize;
                var column = number % boardSize;
                return new Point(row, column);
            })
            .ToArray();
    }
}