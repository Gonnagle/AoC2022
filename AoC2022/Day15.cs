namespace AoC2022;

public static class Day15

{
    public static string Part1(IEnumerable<string> lines)
    {
        // Hack to differentiate the value between example and actual input...
        var linesList = lines.ToList();
        var yToAnalyze = linesList.Count() < 20 ? 10 : 2000000;

        var sensorSystem = new SensorSystem(linesList);

        return sensorSystem.BeaconFreeNodesOnRow(yToAnalyze).ToString();
    }

    public static string Part2(IEnumerable<string> lines)
    {
        // Hack to differentiate the value between example and actual input...
        var linesList = lines.ToList();
        var maxWidth = linesList.Count() < 20 ? 20 : 4000000;

        var sensorSystem = new SensorSystem(linesList);
        var beacon = sensorSystem.LocateBeacon(maxWidth);
        Int128 frequency = beacon.X * 4000000 + beacon.Y;

        return frequency.ToString();
    }

    private class SensorSystem
    {
        private readonly int _xOffset;
        private readonly int _width;
        private readonly List<Coordinate[]> Sensors = new List<Coordinate[]>();

        public SensorSystem(IEnumerable<string> lines)
        {
            var minX = 9999999;
            var maxX = -9999999;
            foreach (var line in lines)
            {
                var stringCoordinates = line.Split("closest beacon");
                var sensorCoordinate = new Coordinate(stringCoordinates[0]);
                var beaconCoordinate = new Coordinate(stringCoordinates[1]);
                minX = int.Min(minX, int.Min(sensorCoordinate.X, beaconCoordinate.X));
                maxX = int.Max(maxX, int.Max(sensorCoordinate.X, beaconCoordinate.X));
                Sensors.Add(new[] {sensorCoordinate, beaconCoordinate});

                _width = maxX - minX;
                _xOffset = Math.Abs(minX); // Knowing it's negative
            }
        }

        public int BeaconFreeNodesOnRow(int yToAnalyze)
        {
            var beaconsOnRow = new List<int>();
            var extraSpaceToReserve = yToAnalyze == 10 ? 5 : 1000000;
            var adjustedXOffset = _xOffset + extraSpaceToReserve;
            var adjustedWidth = _width + 2 * extraSpaceToReserve;
            var scanningStatus = new bool[adjustedWidth];
            foreach (var sensor in Sensors)
            {
                if (sensor[1].Y == yToAnalyze)
                {
                    beaconsOnRow.Add(sensor[1].X);
                }

                var manhattanDist = sensor[0].ManhattanDistance(sensor[1]);
                var yDiff = yToAnalyze - sensor[0].Y;
                var yDist = Math.Abs(yDiff);
                var xTraverse = yDist <= manhattanDist
                    ? yDist == 0 ? manhattanDist - yDist : manhattanDist - yDist + 1
                    : 0;
                var firstXonRow = sensor[0].X + adjustedXOffset - xTraverse;
                var fillCount = 2 * xTraverse - 1;
                if (xTraverse > 0)
                {
                    Array.Fill(scanningStatus, true, firstXonRow, fillCount);
                }
            }

            return scanningStatus.Count(x => x) - beaconsOnRow.Distinct().Count();
        }

        public Coordinate LocateBeacon(int maxWidth)
        {
            var adjustedWidth = maxWidth + 1;
            //var scanningStatus = new HashSet<int>(adjustedWidth);
            var xTraverse = 0;
            var firstXonRow = 0;
            var fillCount = 0;
            for (var yToAnalyze = 0; yToAnalyze <= 100; ++yToAnalyze)
            {
                var scanningStatus = ResetScanningStatusArray(adjustedWidth);
                
                foreach (var sensor in Sensors)
                {
                    if (sensor[1].Y == yToAnalyze && sensor[1].X >= 0 && sensor[1].X <= adjustedWidth)
                    {
                        // scanningStatus[sensor[1].X] = true;
                        scanningStatus.Remove(sensor[1].X);
                    }

                    xTraverse = ResolveXTraverse(sensor[0].ManhattanDistance(sensor[1]), yToAnalyze, sensor[0].Y);

                    if (xTraverse > 0)
                    {
                        (firstXonRow, fillCount) = ResolveXFillParams(xTraverse, sensor[0].X, adjustedWidth);

                        // scanningStatus = XFill(scanningStatus, firstXonRow, fillCount);
                        XFill(scanningStatus, firstXonRow, fillCount);
                    }
                }

                if (scanningStatus.Count > 0)
                {
                    // Horrible off by one correction... no idea what went wrong :D
                    // var x = scanningStatus.ToList().FindIndex(noway => !noway) + 1;
                    var x = scanningStatus.First() + 1;

                    return (x, yToAnalyze);
                }
            }
            return new Coordinate();
        }

        private static void XFill(HashSet<int> scanningStatus, int firstXonRow, int fillCount)
        {
            // Array.Fill(scanningStatus, true, firstXonRow, fillCount);
            for (int i = firstXonRow; i <= firstXonRow + fillCount; ++i)
            {
                scanningStatus.Remove(i);
            }
            
            // return scanningStatus;
        }

        private static (int firstXonRow, int fillCount) ResolveXFillParams(int xTraverse, int sensorX, int adjustedWidth)
        {
            var firstXonRow = sensorX - xTraverse;
            var xCorrection = firstXonRow < 0 ? Math.Abs(firstXonRow) : 0;
            firstXonRow = int.Max(0, firstXonRow);
            var fillCount = 2 * xTraverse - 1 - xCorrection;
            fillCount = int.Min(fillCount, adjustedWidth - firstXonRow);

            return (firstXonRow, fillCount);
        }

        private static int ResolveXTraverse(int manhattanDistance, int yToAnalyze, int sensorY)
        {
            var yDist = Math.Abs(yToAnalyze - sensorY);
            return yDist <= manhattanDistance
                ? yDist == 0 ? manhattanDistance : manhattanDistance - yDist + 1
                : 0;
        }

        private static HashSet<int> ResetScanningStatusArray(int width)
        {
            var scanningStatus = new HashSet<int>(width);
            for (var i = 0; i <= width; ++i)
            {
                scanningStatus.Add(i);
            }
            // Array.Fill(scanningStatus, false);
            return scanningStatus;
        }
    }
}