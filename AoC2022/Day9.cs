using System.Diagnostics;

﻿namespace AoC2022
{
    public static class Day9
    {
        public static string Part1(IEnumerable<string> lines)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var bridge = new Bridge();

            foreach (var line in lines)
            {
                bridge.Move(line);
            }

            var result = bridge.TotalVisitedPositions.ToString();
            Console.WriteLine($"{nameof(Day9)} / {nameof(Part1)} took: {stopWatch.ElapsedMilliseconds} ms");

            return result;
        }

        public static string Part2(IEnumerable<string> lines)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var result = "TODO2".ToString();
            Console.WriteLine($"{nameof(Day9)} / {nameof(Part2)} took: {stopWatch.ElapsedMilliseconds} ms");

            return result;
        }

        internal class Bridge
        {
            public int TotalVisitedPositions { get; private set; }

            private readonly Coordinate _head = new();
            private readonly Coordinate _tail = new();

            private readonly Dictionary<Tuple<int, int>, Position> _positions = new ();

            public void Move(string line)
            {
                var direction = line[0];
                var count = int.Parse(line.Split(" ")[1]);

                for (var i = 0; i < count; ++i)
                {
                    Step(direction);
                }
            }

            public void Step(char direction)
            {
                if (direction == 'U')
                {
                    ++_head.Y;

                    if (_head.Y - _tail.Y == 2)
                    {
                        ++_tail.Y;
                        if (_head.X != _tail.X)
                        {
                            _tail.X = _head.X;
                        }
                    }
                }
                if (direction == 'D')
                {
                    --_head.Y;

                    if (_head.Y - _tail.Y == -2)
                    {
                        --_tail.Y;
                        if (_head.X != _tail.X)
                        {
                            _tail.X = _head.X;
                        }
                    }
                }
                if (direction == 'R')
                {
                    ++_head.X;

                    if (_head.X - _tail.X == 2)
                    {
                        ++_tail.X;
                        if (_head.Y != _tail.Y)
                        {
                            _tail.Y = _head.Y;
                        }
                    }
                }
                if (direction == 'L')
                {
                    --_head.X;

                    if (_head.X - _tail.X == -2)
                    {
                        --_tail.X;
                        if (_head.Y != _tail.Y)
                        {
                            _tail.Y = _head.Y;
                        }
                    }
                }

                var headKey = Tuple.Create(_head.X, _head.Y);
                var tailKey = Tuple.Create(_tail.X, _tail.Y);

                var headPosition = _positions.ContainsKey(headKey) ? _positions[headKey] : _positions[headKey] = new Position();
                var tailPosition = _positions.ContainsKey(tailKey) ? _positions[tailKey] : _positions[tailKey] = new Position();

                headPosition.HeadVisited = true;
                if (tailPosition.TailVisited != true)
                {
                    ++TotalVisitedPositions;
                }
                tailPosition.TailVisited = true;

                //_positions[Tuple.Create(_head.X, _head.Y)] = headPosition;
                //_positions[tailKey] = tailPosition;
            }

            internal class Coordinate
            {
                internal int X { get; set; }
                internal int Y { get; set; }
            }
            internal class Position
            {
                internal bool HeadVisited { get; set; }
                internal bool TailVisited { get; set; }
            }
        }
    }
}

