public static class StringExtensions
{
    public static string AlignToCenter(this string str, int symbolsCount)
    {
        if (str.Length >= symbolsCount)
        {
            return str;
        }

        int leftPadding = (symbolsCount - str.Length) / 2;
        int rightPadding = symbolsCount - str.Length - leftPadding;

        return new string(' ', leftPadding) + str + new string(' ', rightPadding);
    }
}