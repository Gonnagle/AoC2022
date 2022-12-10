using System.Diagnostics;

﻿namespace AoC2022
{
    public static class Day10
    {
        public static string Part1(IEnumerable<string> lines)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var processor = new Processor();

            foreach (var line in lines)
            {
                processor.ProcessInstruction(line);
            }

            var result = processor.SumOfSignalStrengths.ToString();
            Console.WriteLine($"{nameof(Day10)} / {nameof(Part1)} took: {stopWatch.ElapsedMilliseconds} ms");

            return result;
        }

        public static string Part2(IEnumerable<string> lines)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var processor = new Processor();

            foreach (var line in lines)
            {
                processor.ProcessInstruction(line);
            }

            var result = processor.SumOfSignalStrengths.ToString();
            Console.WriteLine($"{nameof(Day10)} / {nameof(Part2)} took: {stopWatch.ElapsedMilliseconds} ms");

            return result;
        }

        internal class Processor
        {
            private int _currentCycle = 0;
            private int _registerX = 1;
            internal int SumOfSignalStrengths;

            internal void ProcessInstruction(string instruction)
            {
                if(instruction.StartsWith("noop")){
                    Tick();
                }
                else{
                    var addx = int.Parse(instruction.Split(" ")[1]);
                    Tick(2);
                    _registerX += addx;
                }
            }

            private void Tick(int amount = 1){
                for(var i = 0; i < amount; ++i){
                    ++_currentCycle;
                    if(_currentCycle == 20 || (_currentCycle - 20) % 40 == 0){
                        var signalStrength = _currentCycle * _registerX;
                        Console.WriteLine($"Signal strength: {signalStrength}");
                        SumOfSignalStrengths += signalStrength;
                    }
                }
            }
        }
    }
}

