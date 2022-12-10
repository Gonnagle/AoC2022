namespace AoC2022
{
    public static class Day10
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var processor = new Processor();
            foreach (var line in lines)
            {
                processor.ProcessInstruction(line);
            }
            var result = processor.SumOfSignalStrengths.ToString();
            return result;
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var processor = new Processor();
            foreach (var line in lines)
            {
                processor.ProcessInstruction(line);
            }
            var result = processor.CRT.ToString();
            return result;
        }

        internal class Processor
        {
            private int _currentCycle = 0;
            private int _registerX = 1;
            internal string CRT = string.Empty;
            internal int SumOfSignalStrengths;

            internal void ProcessInstruction(string instruction)
            {
                if(instruction.StartsWith("noop")){
                    Tick();
                }
                else
                {
                    var addx = int.Parse(instruction.Split(" ")[1]);
                    Tick(2);
                    _registerX += addx;
                }
            }

            private void Tick(int amount = 1)
            {
                for(var i = 0; i < amount; ++i)
                {
                    DrawCRT();
                    ++_currentCycle;
                    if(_currentCycle == 20 || (_currentCycle - 20) % 40 == 0)
                    {
                        var signalStrength = _currentCycle * _registerX;
                        Console.WriteLine($"Signal strength: {signalStrength}");
                        SumOfSignalStrengths += signalStrength;
                    }
                }
            }

            private void DrawCRT()
            {
                var xPosition = _currentCycle % 40;

                if(_currentCycle != 0 && xPosition == 0)
                {
                    CRT += '\n';
                }

                if(_registerX - 1 <= xPosition && xPosition <= _registerX + 1)
                {
                    CRT += '#';
                }
                else{
                    CRT += '.';
                }
            }
        }
    }
}

