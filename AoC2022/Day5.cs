using System.Text.RegularExpressions;

namespace AoC2022
{
    public static class Day5
    {
        public record MoveCommand(int Count, int SourceStack, int TargetStack);

        public static string Part1(IEnumerable<string> lines)
        {
            var stacks = ResolveStacksAndCommands(lines, out var moveCommands);

            foreach (var moveCommand in moveCommands)
            {
                for (var i = 0; i < moveCommand.Count; i++)
                {
                    var item = stacks[moveCommand.SourceStack].First();
                    stacks[moveCommand.SourceStack].RemoveFirst();
                    stacks[moveCommand.TargetStack].AddFirst(item);
                }
            }

            return stacks.OrderBy(x => x.Key)
                .Aggregate(string.Empty, (current, stack) => current + stack.Value.First());
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var stacks = ResolveStacksAndCommands(lines, out var moveCommands);

            foreach (var moveCommand in moveCommands)
            {
                var items = stacks[moveCommand.SourceStack].Take(0..moveCommand.Count);
                foreach(var item in items.Reverse())
                {
                    stacks[moveCommand.TargetStack].AddFirst(item);
                }

                for (var i = 0; i < moveCommand.Count; i++)
                {
                    stacks[moveCommand.SourceStack].RemoveFirst();
                }
            }

            return stacks.OrderBy(x => x.Key)
                .Aggregate(string.Empty, (current, stack) => current + stack.Value.First());
        }

        private static Dictionary<int, LinkedList<char>> ResolveStacksAndCommands(IEnumerable<string> lines,
            out List<MoveCommand> moveCommands)
        {
            var stacks = new Dictionary<int, LinkedList<char>>();
            moveCommands = new List<MoveCommand>();

            foreach (var line in lines)
            {
                if ((line.StartsWith("[") || line.StartsWith(" ")) && !line.StartsWith(" 1"))
                {
                    var stackIndex = 1;
                    for (var i = 1; i < line.Length; i += 4)
                    {
                        var item = line[i];
                        if (!string.IsNullOrWhiteSpace(item.ToString()))
                        {
                            if (!stacks.ContainsKey(stackIndex))
                            {
                                stacks[stackIndex] = new LinkedList<char>();
                            }

                            stacks[stackIndex].AddLast(item);
                        }
                        ++stackIndex;
                    }
                }
                else if (line.StartsWith("move"))
                {
                    const string moveCommandPattern = "move (\\d+) from (\\d+) to (\\d+)";
                    var moveCommandRegex = new Regex(moveCommandPattern, RegexOptions.IgnoreCase);
                    var match = moveCommandRegex.Match(line);
                    var count = int.Parse(match.Groups[1].Value);
                    var sourceStack = int.Parse(match.Groups[2].Value);
                    var targetStack = int.Parse(match.Groups[3].Value);

                    moveCommands.Add(new MoveCommand(count, sourceStack, targetStack));
                }
            }
            return stacks;
        }
    }
}
