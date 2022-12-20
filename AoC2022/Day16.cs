using System.Text.RegularExpressions;
using System.IO;

namespace AoC2022;

public static class Day16

{
    public static string Part1(IEnumerable<string> lines)
    {
        var volcano = new Volcano(lines);

        volcano.OutputGraphFile().Wait();

        return "TODO".ToString();
    }

    public static string Part2(IEnumerable<string> lines)
    {
        return "TODO".ToString();
    }

    private class Volcano
    {
        private IDictionary<string, Valve> Valves { get; } = new Dictionary<string, Valve>();

        public Volcano(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                const string valveRegex = @"Valve (..) has flow rate=(\d+); tunnels? leads? to valves? (.*)$";
                var match = Regex.Match(line, valveRegex);
                 
                var name = match.Groups[1].Value;
                var flowRate = int.Parse(match.Groups[2].Value);
                var tunnels = match.Groups[3].Value.Split(", ");

                var valve = new Valve(name, flowRate);
                Valves[name] = valve;

                foreach (var tunnel in tunnels)
                {
                    Valve tunnelTarget;
                    if (Valves.ContainsKey(tunnel))
                    {
                        tunnelTarget = Valves[tunnel];
                    }
                    else
                    {
                        tunnelTarget = new Valve(tunnel);
                        Valves[tunnel] = tunnelTarget;;
                    }
                    valve.Tunnels.Add(tunnelTarget);
                }
            }
        }

        public async Task OutputGraphFile()
        {
            var lines = new List<string>();
            lines.Add("strict graph Volcano {");
            foreach (var valve in Valves)
            {
                lines.Add(valve.Value.GetGraphDefinition());
            }

            lines.Add("}");
            await File.WriteAllLinesAsync("graph.dot", lines);
        }
    }

    private class Valve
    {
        public string Name { get; }
        public IList<Valve> Tunnels { get; } = new List<Valve>();
        public int FlowRate { get; set; }

        public Valve(string name, int flowRate = 0)
        {
            Name = name;
            FlowRate = flowRate;
        }

        public string GetGraphDefinition()
        {
            var tunnels = string.Join(" ", Tunnels.Select(m => m.Name).ToArray());
            return $"\t{Name} [style=\"filled\" fillcolor=\"#{FlowRate.ToString().PadLeft(2, '0')}FFFF\" label=\"\\N\\n{FlowRate}\"]\n\t{Name} -- {{ {tunnels} }};";
        }
    }
}