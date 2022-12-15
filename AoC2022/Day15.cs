namespace AoC2022
{
    public static class Day15

    {
        public static string Part1(IEnumerable<string> lines)
        {
            var sensorSystem = new SensorSystem(lines);

            var result = "TODO";

            return result.ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var scanner = new SensorSystem(lines);

            var result = "TODO";

            return result.ToString();
        }

        private class SensorSystem
        {
            private int _minX;
            private int _maxX;
            private int _xOffset;
            private bool[] rowOfInterest;

            public SensorSystem(IEnumerable<string> input)
            {
                var sensors = new List<Coordinate[]>();
                foreach (var row in input)
                {
                    var pathLine = new List<int[]>();
                    var coordinates = row.Split(" -> ");
                    foreach (var coordinate in coordinates)
                    {

                    }
                }
            }
        }
    }
}

