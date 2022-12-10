namespace AoC2022
{
    public static class Day8
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var treeMap = new TreeMap(lines.ToArray());
            var result = treeMap.ResolveTreeVisibilities().ToString();
            return result;
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var treeMap = new TreeMap(lines.ToArray());
            var result = treeMap.ResolveHighestScenicScore().ToString();
            return result;
        }

        public class TreeMap
        {
            public Tree[,] Map { get; }
            public int MapHeight { get; }
            public int MapWidth { get; }

            public TreeMap(string[] treeLines)
            {
                _ = treeLines ?? throw new ArgumentNullException(nameof(treeLines));
                MapWidth = treeLines.First().Length;
                MapHeight = treeLines.Length;

                Map = new Tree[MapWidth, MapHeight];

                for (var y = 0; y < MapHeight; ++y)
                {
                    var treeLine = treeLines[y];
                    for (var x = 0; x < MapWidth; ++x)
                    {
                        Map[x, y] = new Tree(x, y, int.Parse(treeLine[x].ToString()));
                    }
                }
            }

            public Tree[] TreesOnLeft(int x, int y) => GetRowSlice(y, 0, x - 1);
            public Tree[] TreesOnRight(int x, int y) => GetRowSlice(y, x + 1, MapWidth - 1);
            public Tree[] TreesOnTop(int x, int y) => GetColumnSlice(x, 0, y - 1);
            public Tree[] TreesOnBelow(int x, int y) => GetColumnSlice(x, y + 1, MapHeight - 1);

            public int ResolveTreeVisibilities()
            {
                var visibleTrees = 0;

                for (var y = 0; y < MapHeight; ++y)
                {
                    for (var x = 0; x < MapWidth; ++x)
                    {
                        if (x == 0 || y == 0 || x == MapWidth - 1 || y == MapHeight - 1)
                        {
                            Map[x, y].Visible = true;
                        }
                        else
                        {
                            if (TreesOnLeft(x, y).All(t => t.Height < Map[x, y].Height))
                            {
                                Map[x, y].Visible = true;
                            }
                            else if (TreesOnRight(x, y).All(t => t.Height < Map[x, y].Height))
                            {
                                Map[x, y].Visible = true;
                            }
                            else if (TreesOnTop(x, y).All(t => t.Height < Map[x, y].Height))
                            {
                                Map[x, y].Visible = true;
                            }
                            else if (TreesOnBelow(x, y).All(t => t.Height < Map[x, y].Height))
                            {
                                Map[x, y].Visible = true;
                            }
                            else
                            {
                                Map[x, y].Visible = false;
                            }
                        }

                        if ((bool) Map[x, y].Visible!)
                        {
                            ++visibleTrees;
                        }
                    }
                }
                return visibleTrees;
            }

            public int ResolveHighestScenicScore()
            {
                var highestScenicScore = 0;

                for (var y = 0; y < MapHeight; ++y)
                {
                    for (var x = 0; x < MapWidth; ++x)
                    {
                        Map[x, y].ResolveScenicScore(this);

                        if (Map[x, y].ScenicScore > highestScenicScore)
                        {
                            highestScenicScore = Map[x, y].ScenicScore;
                        }
                    }
                }
                return highestScenicScore; ;
            }

            // Based on https://stackoverflow.com/a/51241629
            public Tree[] GetColumnSlice(int columnNumber, int firstIndex, int lastIndex)
            {
                return Enumerable.Range(firstIndex, lastIndex - firstIndex + 1)
                    .Select(y => Map[columnNumber, y])
                    .ToArray();
            }

            public Tree[] GetRowSlice(int rowNumber, int firstIndex, int lastIndex)
            {
                var slice = Enumerable.Range(firstIndex, lastIndex - firstIndex + 1)
                    .Select(x => Map[x, rowNumber])
                    .ToArray();
                return slice;
            }
        }

        public class Tree
        {
            public int X { get; }
            public int Y { get; }
            public int Height { get; }
            public bool? Visible { get; set; }
            public int ScenicScore { get; private set; }

            public Tree(int x, int y, int height, bool? visible = null)
            {
                X = x;
                Y = y;
                Height = height;
                Visible = visible;
            }

            public void ResolveScenicScore(TreeMap treeMap)
            {
                var scores = new int[]
                {
                    ResolveScenicScoreComponent(treeMap.TreesOnLeft(X, Y).Reverse().ToArray()),
                    ResolveScenicScoreComponent(treeMap.TreesOnRight(X, Y).ToArray()),
                    ResolveScenicScoreComponent(treeMap.TreesOnTop(X, Y).Reverse().ToArray()),
                    ResolveScenicScoreComponent(treeMap.TreesOnBelow(X, Y).ToArray())
                };

                ScenicScore = scores.Aggregate((a, b) => a * b);
            }

            private int ResolveScenicScoreComponent(IReadOnlyCollection<Tree> treesInDirection)
            {
                var maximumVisibility = treesInDirection.Count;
                var treesUntilLast = treesInDirection.TakeWhile(tree => tree.Height < Height).Count();
                return treesUntilLast < maximumVisibility ? treesUntilLast + 1 : maximumVisibility;
            }
        }
    }
}

