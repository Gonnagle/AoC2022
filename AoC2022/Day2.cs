namespace AoC2022
{
    public static class Day2
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var totalScore = 0;

            foreach (var match in lines)
            {
                var parts = match.Split(" ");
                var elf = parts[0][0] - 'A';
                var me = parts[1][0] - 'X';

                var result = me - elf;
                var score = me + 1;

                if (result is 1 or -2)
                {
                    score += 6;
                }
                else if (result == 0)
                {
                    score += 3;
                }
                totalScore += score;
            }
            return totalScore.ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var totalScore = 0;

            foreach (var match in lines)
            {
                var parts = match.Split(" ");
                var elf = parts[0][0] - 'A';
                var outcome = parts[1][0];

                var me = outcome switch
                {
                    'Z' => elf + 1 <= 2 ? elf + 1 : 0,   // Z -> Win
                    'Y' => elf,                          // Y -> Draw
                    'X' => elf - 1 >= 0 ? elf - 1: 2     // X -> Loose
                };
                var result = me - elf;
                var score = me + 1;

                if (result is 1 or -2)
                {
                    score += 6;
                }
                else if (result == 0)
                {
                    score += 3;
                }
                totalScore += score;
            }
            return totalScore.ToString();
        }
    }
}