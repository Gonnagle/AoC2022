namespace AoC2022
{
    public static class Day4
    {
        public static string Part1(IEnumerable<string> pairs)
        {
            var count = 0;
            foreach (var pair in pairs)
            {
                var splitPairs = pair.Split(",");
                var pair1 = splitPairs[0];
                var pair2 = splitPairs[1];
                var assignment1 = new Assignment(pair1);
                var assignment2 = new Assignment(pair2);

                if (assignment1.IsWithin(assignment2) || assignment2.IsWithin(assignment1))
                {
                    ++count;
                }
            }
            return count.ToString();
        }

        public static string Part2(IEnumerable<string> pairs)
        {
            var count = 0;
            foreach (var pair in pairs)
            {
                var splitPairs = pair.Split(",");
                var pair1 = splitPairs[0];
                var pair2 = splitPairs[1];
                var assignment1 = new Assignment(pair1);
                var assignment2 = new Assignment(pair2);

                if (assignment1.Overlaps(assignment2) || assignment2.Overlaps(assignment1))
                {
                    ++count;
                }
            }
            return count.ToString();
        }
    }

    public class Assignment
    {
        public int FirstSection { get; }
        public int LastSection { get; }

        public Assignment(string stringAssignment)
        {
            var assignmentParts = stringAssignment.Split("-");
            FirstSection = int.Parse(assignmentParts[0]);
            LastSection = int.Parse(assignmentParts[1]);
        }

        public bool IsWithin(Assignment other)
        {
            return FirstSection >= other.FirstSection && LastSection <= other.LastSection;
        }

        public bool Overlaps(Assignment other)
        {
            return (FirstSection >= other.FirstSection && FirstSection <= other.LastSection)
                   || (LastSection >= other.FirstSection && LastSection <= other.LastSection);
        }
    }
}
