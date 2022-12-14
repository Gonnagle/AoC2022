namespace AoC2022
{
    public static class Day14
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var scanner = new Scanner(lines);
            var result = scanner.Cave.PourSandUntilStable();

            // scanner.Cave.Print();

            return result.ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var scanner = new Scanner(lines, true);

            var result = scanner.Cave.PourSandUntilStableBedrockEdition();

            // scanner.Cave.Print();

            return result.ToString();
        }

        internal class Scanner
        {
            public int[] mins = new int[] { 99999, 99999 };
            public int[] maxs = new int[] { 0, 0 };
            private readonly Cave.Pixel _start;

            public Cave Cave { get; }

            internal Scanner(IEnumerable<string> input, bool expectBedrock = false)
            {
                var pathWays = new List<List<int[]>>();
                foreach (var row in input)
                {
                    var pathLine = new List<int[]>();
                    var coordinates = row.Split(" -> ");
                    foreach (var coordinate in coordinates)
                    {
                        var unparsed = coordinate.Split(",");
                        var x = int.Parse(unparsed[0]);
                        var y = int.Parse(unparsed[1]);

                        // There probably would be neat way to combine these with some array operations or so
                        // but can't quite my wrap my head around that atm
                        mins[0] = int.Min(x, mins[0]);
                        maxs[0] = int.Max(x, maxs[0]);
                        maxs[1] = int.Max(y, maxs[1]);
                        pathLine.Add(new int[] { x, y });
                    }
                    pathWays.Add(pathLine);
                }
                var xOffset = mins[0] - 250;
                var width = maxs[0] - mins[0] + 500;
                var height = maxs[1] + 1;

                Cave = new Cave(xOffset, width, height, 500, expectBedrock);

                // With more clever ordering of things we could avoid this second iteration, but...
                foreach (var pathWay in pathWays)
                {
                    // Cleaner way to iterate through consecutive pair in list?
                    for (var i = 0; i < pathWay.Count - 1; ++i)
                    {
                        var start = pathWay[i];
                        var end = pathWay[i + 1];

                        Cave.AddPath(start, end);
                    }

                }

                _start = Cave.GetPixel(500, 0);
                _start.ChangeToStart();
            }
        }

        internal class Cave
        {
            internal Pixel[,] TwoDimMap { get; }
            internal Pixel Start { get; }

            private int _xOffset;
            private int _width;
            private int _height;

            internal Cave(int xOffset, int width, int height, int xStart, bool addBedRock = false)
            {
                _xOffset = xOffset;
                _width = width;
                _height = height;
                _height = addBedRock ? _height + 2 : _height;

                TwoDimMap = EmptyMap(_width, _height);
                 if(addBedRock)
                 {
                    AddPath(new int[] {0, _height - 1}, new int[] {width - 1, _height - 1}, true);
                 }

                Start = GetPixel(xStart, 0);
            }

            internal Pixel[,] EmptyMap(int width, int height)
            {
                var map = new Pixel[width, height];

                for (var y = 0; y < height; ++y)
                {
                    for (var x = 0; x < width; ++x)
                    {
                        map[x, y] = new Pixel(x, y);
                    }
                }
                return map;
            }

            internal Pixel GetPixel(int x, int y){
                return TwoDimMap[x - _xOffset, y];
            }

            internal Pixel GetRelativePixel(int x, int y){
                return TwoDimMap[x, y];
            }

            internal void AddPath(int[] start, int[] end, bool transformedPixels = false)
            {
                var xStep = end[0] - start[0] < 0 ? -1 : 1;
                var yStep = end[1] - start[1] < 0 ? -1 : 1;

                for (var x = start[0]; x != end[0] + xStep; x += xStep)
                {
                    (transformedPixels 
                        ? GetRelativePixel(x, start[1])
                        : GetPixel(x, start[1])).ChangeToStone();
                }

                for (var y = start[1]; y != end[1] + yStep; y += yStep)
                {
                    (transformedPixels 
                        ? GetRelativePixel(start[0], y)
                        : GetPixel(start[0], y)).ChangeToStone();
                }
            }

            internal int PourSandUntilStable()
            {
                var sandGrainsTotal = 0;
                var current = Start;

                while(current.Y < _height)
                {
                    current = Start;
                    Pixel? next = null;
                    var atRest = false;
                    while(!atRest)
                    {
                        if(current.Y == _height - 1)
                        {
                            return sandGrainsTotal;
                        }
                        next = DropStep(current);
                        if(next == current){
                            current.ChangeToSand();
                            atRest = true;
                            ++sandGrainsTotal;
                        }
                        else
                        {
                            current = next;
                        }
                    }
                }
                return -1;
            }

            internal int PourSandUntilStableBedrockEdition()
            {
                var sandGrainsTotal = 0;
                var current = Start;

                while(current.Y < _height)
                {
                    current = Start;
                    Pixel? next = null;
                    var atRest = false;
                    while(!atRest)
                    {
                        if(current == Start && current.Contents == 'o')
                        {
                            return sandGrainsTotal;
                        }
                        next = DropStep(current);
                        if(next == current){
                            current.ChangeToSand();
                            atRest = true;
                            ++sandGrainsTotal;
                        }
                        else
                        {
                            current = next;
                        }
                    }
                }
                return -1;
            }

            private Pixel DropStep(Pixel pixel)
            {
                var newY = pixel.Y + 1;
                var nextCandidate = GetRelativePixel(pixel.X, newY);

                if(nextCandidate.Contents == '.'){
                    return nextCandidate;
                }

                nextCandidate = GetRelativePixel(pixel.X - 1, newY);
                if(nextCandidate.Contents == '.'){
                    return nextCandidate;
                }

                nextCandidate = GetRelativePixel(pixel.X + 1, newY);
                if(nextCandidate.Contents == '.'){
                    return nextCandidate;
                }

                return pixel;
            }

            internal void Print()
            {
                for (var y = 0; y < _height; ++y)
                {
                    var printLine = y.ToString().PadLeft(3, ' ');
                    for (var x = 0; x < _width; ++x)
                    {
                        printLine += TwoDimMap[x, y].Contents;
                    }
                    Console.WriteLine(printLine);
                }
            }

            internal class Pixel
            {
                internal char Contents { get; private set; } = '.';
                internal int X { get; }
                internal int Y { get; }

                internal Pixel(int x, int y)
                {
                    X = x;
                    Y = y;
                }

                internal void ChangeToStone()
                {
                    Contents = '#';
                }

                internal void ChangeToStart()
                {
                    Contents = '+';
                }

                internal void ChangeToSand()
                {
                    Contents = 'o';
                }
            }
        }
    }
}

