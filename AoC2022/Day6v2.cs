namespace AoC2022
{
    public static class Day6v2
    {
        public static string Part1(IEnumerable<string> lines)
            => FindFirstSequence(4, lines.First()).ToString();

        public static string Part2(IEnumerable<string> lines)
            => FindFirstSequence(14, lines.First()).ToString();

        public static int FindFirstSequence(int windowSize, string input)
        {
            for(var i = 0; i + windowSize <= input.Length; i++)
            {
                if(input[i..(i+windowSize)].ToCharArray().ToList().Distinct().Count() == windowSize)
                {
                    return i + windowSize;
                }
            }
            return -1;
        }
    }
}
