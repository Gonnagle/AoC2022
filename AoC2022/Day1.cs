namespace AoC2022
{
    public static class Day1
    {
        public static string Part1(IEnumerable<string> lines)
        {
            //const string filePath = "inputs/day1.txt";

            //var lines = File.ReadLines(filePath);
            var current = 0;
            var biggest = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (current > biggest)
                    {
                        biggest = current;
                    }
                    current = 0;
                }
                else
                {
                    current += int.Parse(line);
                }
            }
            if (current > biggest)
            {
                biggest = current;
            }

            return biggest.ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            //const string filePath = "inputs/day1.txt";
            //var lines = File.ReadLines(filePath);
            var current = 0;
            var all = new List<int>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    all.Add(current);
                    current = 0;
                }
                else
                {
                    current += int.Parse(line);
                }
            }
            all.Add(current);
            all.Sort();
            var top3 = all.TakeLast(3);
            var result = top3.Sum();

            return result.ToString();
        }
    }
}
