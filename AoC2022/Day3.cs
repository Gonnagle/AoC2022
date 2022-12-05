namespace AoC2022
{
    public static class Day3
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var sum = 0;
            foreach (var line in lines)
            {
                var splitPosition = line.Length / 2;
                var firstPart = line[..splitPosition].ToCharArray();
                var lastPart = line[splitPosition..].ToCharArray();
                var common = firstPart.Intersect(lastPart).First();
                var priority = char.IsUpper(common) ? common - 'A' + 27 : common - 'a' + 1;
                sum += priority;
            }
            return sum.ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var sum = 0;
            var group = new List<char[]>();
            foreach (var line in lines)
            {
                group.Add(line.ToCharArray());

                if (group.Count == 3)
                {
                    var badge = group[0].Intersect(group[1]).Intersect(group[2]).First();
                    var priority = char.IsUpper(badge) ? badge - 'A' + 27 : badge - 'a' + 1;
                    sum += priority;
                    group.Clear();
                }
            }
            return sum.ToString();
        }
    }
}
