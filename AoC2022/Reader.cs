namespace AoC2022
{
    public static class Reader
    {
        public static IEnumerable<string> ReadLinesIntoEnumerable(string filePath) =>
            File.ReadLines(filePath);
    }
}