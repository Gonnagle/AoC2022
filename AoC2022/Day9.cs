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
            
            var bridge = new Bridge(10);

            foreach (var line in lines)
            {
                bridge.Move(line);
            }

            var result = bridge.TotalVisitedPositions.ToString();
            Console.WriteLine($"{nameof(Day9)} / {nameof(Part2)} took: {stopWatch.ElapsedMilliseconds} ms");

            return result;
        }

        internal class Bridge
        {
            public int TotalVisitedPositions { get; private set; }

            private readonly Coordinate[] _knots;

            private readonly Dictionary<Tuple<int, int>, Position> _positions = new ();

            public Bridge(int ropeLength = 2){
                _knots = new Coordinate[ropeLength];
                for(var i = 0; i < ropeLength; ++i){
                    _knots[i] = new Coordinate(0, 0);
                }
            }

            public void Move(string line)
            {
                
                var direction = line[0];
                var count = int.Parse(line.Split(" ")[1]);

                for (var i = 0; i < count; ++i)
                {
                    Step(direction);
                }
            }

            private void Step(char direction)
            {
                var head = _knots[0];
                
                switch( direction )
                {
                   case 'U': head.MoveUp(); break;
                   case 'D': head.MoveDown(); break;
                   case 'R': head.MoveRight(); break;
                   case 'L': head.MoveLeft(); break;
                };

                for(var i = 0; i < _knots.Length - 1; ++i){
                    TransformTail(_knots[i], _knots[i + 1]);
                }

                var tail = _knots[^1];
                var tailPosition = _positions.ContainsKey(tail.Key) ? _positions[tail.Key] : _positions[tail.Key] = new Position();
                if (tailPosition.TailVisited != true)
                {
                    ++TotalVisitedPositions;
                }
                tailPosition.TailVisited = true;
            }

            private void TransformTail(Coordinate head, Coordinate tail){
                var xDiff = head.X - tail.X;
                var yDiff = head.Y - tail.Y;

                var xMove = 0;
                var yMove = 0;

                if(Math.Abs(xDiff) == 2 || Math.Abs(yDiff) == 2){
                    // Direct transition
                    if(xDiff == 0 || yDiff == 0){
                        xMove = xDiff / 2;
                        yDiff = yDiff / 2;
                    }
                    // Diagonal transition
                    xMove = Math.Sign(xDiff);
                    yMove = Math.Sign(yDiff);
                }
                tail.X += xMove;
                tail.Y += yMove;
            }

            internal class Coordinate
            {
                internal int X { get; set; }
                internal int Y { get; set; }
                
                internal Tuple<int, int> Key => Tuple.Create(X, Y);
                
                internal Coordinate(int x, int y){
                    X = x;
                    Y = y;
                }

                internal void MoveLeft() => --X;
                internal void MoveRight() => ++X;
                internal void MoveDown() => --Y;
                internal void MoveUp() => ++Y;
            }
            internal class Position
            {
                internal bool HeadVisited { get; set; }
                internal bool TailVisited { get; set; }
            }
        }
    }
}

