namespace AoC2022
{
    public static class Day11
    {
        public static string Part1(IEnumerable<string> lines)
        {
            return SimianShenanigans.MonkeyBusiness(lines, 20, 3).ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            return SimianShenanigans.MonkeyBusiness(lines, 10000, 1, true).ToString();
        }

        internal static class SimianShenanigans
        {
            internal static Int128 MonkeyBusiness(IEnumerable<string> lines, int rounds, int worryDivider, bool useModder = false)
            {
                var monkeys = new Dictionary<int, Monkey>();
                var monkeyDefinition = new List<string>();
                var monkeyId = 0;
                foreach (var line in lines)
                {
                    if(line == string.Empty)
                    {
                        monkeys.Add(monkeyId, new Monkey(monkeyDefinition.ToArray(), monkeys, worryDivider));
                        monkeyDefinition.Clear();
                        ++monkeyId;
                    }
                    else
                    {
                        monkeyDefinition.Add(line);
                    }
                }
                monkeys.Add(monkeyId, new Monkey(monkeyDefinition.ToArray(), monkeys));

                var monkeyIds = monkeys.Keys.ToList();
                monkeyIds.Sort();

                var mod = useModder ? monkeys.Values.Select(x => x.DivisibleBy).Aggregate((a, b) => a * b) : 0;
                for(var i = 0; i < rounds; ++i)
                {
                    foreach(var id in monkeyIds)
                    {
                        monkeys[id].InspectAndThrowItems(mod, useModder);
                    }
                }

                var sortedInspectionCounts = monkeys.Select(x => x.Value.AmountOfInspectedItems).ToList();
                sortedInspectionCounts.Sort();
                sortedInspectionCounts.Reverse();
                Int128 levelOfMonkeyBusiness = sortedInspectionCounts[0] * sortedInspectionCounts[1];
                return levelOfMonkeyBusiness;
            }
        }

        internal class Monkey
        {
            private IList<Int128> _items;
            public Func<Int128, Int128> _operation;
            public int DivisibleBy { get; set; }
            private int _trueTarget;
            private int _falseTarget;
            private int _worryDivider;
            private Dictionary<int, Monkey> _allMonkeys;

            public Int128 AmountOfInspectedItems {get; private set;}

            internal Monkey(string[] monkeyDefinition, Dictionary<int, Monkey> monkeys, int worryDiveder = 3)
            {
                _allMonkeys = monkeys;
                _worryDivider = worryDiveder;
                _items = monkeyDefinition[1].Split("items: ")[1].Split(", ").Select(x => Int128.Parse(x)).ToList();
                _operation = ResolveOperation(monkeyDefinition[2].Split("new = ")[1]);
                DivisibleBy = int.Parse(monkeyDefinition[3].Split("divisible by ")[1]);
                _trueTarget = int.Parse(monkeyDefinition[4].Split("to monkey ")[1]);
                _falseTarget = int.Parse(monkeyDefinition[5].Split("to monkey ")[1]);
            }

            internal void InspectAndThrowItems(int modder = 0, bool useModder = false)
            {
                foreach(var item in _items)
                {
                    ++AmountOfInspectedItems;
                    var worryLevel = _operation(item);
                    worryLevel = useModder ? worryLevel % modder : worryLevel / _worryDivider;

                    if(worryLevel % DivisibleBy == 0)
                    {
                        _allMonkeys[_trueTarget].Catch(worryLevel);
                    }
                    else
                    {
                        _allMonkeys[_falseTarget].Catch(worryLevel);
                    }
                }
                _items.Clear();
            }

            internal void Catch(Int128 item){
                if(item < 0 ){
                    throw new ArgumentOutOfRangeException("OVERFLOW!");
                }
                _items.Add(item);
            }

            private Func<Int128, Int128> ResolveOperation(string funcDef)
            {
                var defParts = funcDef.Split(" ");
                var oper = defParts[1];
                var target = defParts[2];
                
                if(oper == "*")
                {
                    if(target == "old")
                    {
                        return (old) => old * old;
                    }
                    return (old) => old * Int128.Parse(target);
                }
                else
                {
                    if(target == "old")
                    {
                        return (old) => old + old;
                    }
                    return (old) => old + Int128.Parse(target);
                }
            }
        }
    }
}

