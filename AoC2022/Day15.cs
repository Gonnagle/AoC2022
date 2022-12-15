namespace AoC2022
{
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
            var scanner = new SensorSystem(lines);

            var result = "TODO";

            return result.ToString();
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
                    Sensors.Add(new [] {sensorCoordinate, beaconCoordinate});

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
                    var xTraverse = yDist <= manhattanDist ? yDist == 0 ? manhattanDist - yDist : manhattanDist - yDist + 1 : 0;
                    var firstXonRow = sensor[0].X + adjustedXOffset - xTraverse;
                    var fillCount = 2 * xTraverse - 1;
                    if (xTraverse > 0)
                    {
                        Array.Fill(scanningStatus, true, firstXonRow, fillCount);
                    }
                }
                return scanningStatus.Count(x => x) - beaconsOnRow.Distinct().Count();
            }
        }
    }
}

