namespace AoC2022
{
    public static class Day12
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var heightMap = new HeightMap(lines.ToList());

            return heightMap.ResolveShortestPathFrom().ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var minPath = 999999;

            var heightMap = new HeightMap(lines.ToList());

            foreach(var lowPoint in heightMap.LowPoints)
            {
                // Would be more efficent to tweak the resolver a bit to support finding path from ens to start (first lowest)
                // But this was faster to implement...
                heightMap.Reset(); 

                var candidateDistance = heightMap.ResolveShortestPathFrom(lowPoint);
                minPath = int.Min(minPath, candidateDistance);
            }
            return minPath.ToString();
        }

        internal class HeightMap
        {
            private Node[,] _matrix;
            private Node _start;
            private Node _end;

            public IList<Node> LowPoints = new List<Node>(); 

            public HeightMap(IList<string> rows)
            {
                _matrix = new Node[rows.First().Length, rows.Count];

                for(var y = 0; y < rows.Count; ++y)
                {
                    var row = rows[y];
                    for(var x = 0; x < row.Length; ++x)
                    {
                        Node newNode;
                        var mapChar = row[x];
                        if(mapChar == 'S')
                        {
                            newNode = new Node(0);
                            _start = newNode;
                        }
                        else if(mapChar == 'E')
                        {
                            newNode = new Node('z'-'a', true);
                            _end = newNode;
                        }
                        else
                        {
                            newNode = new Node(mapChar - 'a');
                        }
                        
                        if(x > 0)
                        {
                            var before = _matrix[x-1,y];
                            newNode.AddDestinationIfReachable(before);
                            before.AddDestinationIfReachable(newNode);
                        }
                        if(y > 0)
                        {
                            var onTop = _matrix[x,y-1];
                            newNode.AddDestinationIfReachable(onTop);
                            onTop.AddDestinationIfReachable(newNode);
                        }
                        _matrix[x,y] = newNode;

                        if(newNode.Height == 0){
                            LowPoints.Add(newNode);
                        }
                    }
                }
                if(_end == null || _start == null)
                {
                    throw new ArgumentException();
                }
            }

            public int ResolveShortestPathFrom(Node? start = null)
            {
                start = start ?? _start;

                var nextNodes = new Queue<Node>();
                start.MinDistanceFromStart = 0;
                nextNodes.Enqueue(start);

                while(nextNodes.Count > 0)
                {
                    var nodeToInspect = nextNodes.Dequeue();
                    if(nodeToInspect.Visited){
                        continue;
                    }

                    nodeToInspect.Visited = true;

                    nodeToInspect.MinDistanceFromStart = int.Min(nodeToInspect.MinDistanceFromStart, (nodeToInspect.Sources.Min(x => x.MinDistanceFromStart) + 1));

                    if(nodeToInspect.End){
                        return nodeToInspect.MinDistanceFromStart;
                    }
                    foreach(var destination in nodeToInspect.Destinations.Where(x => !x.Visited)){
                        nextNodes.Enqueue(destination);
                    }
                }
                return 9999999;
            }

            public void Reset()
            {
                foreach(var node in _matrix!)
                {
                    node.Reset();
                }
            }
        }

        internal class Node 
        {
            const int NotDiscoveredDistance = 9999999;

            public IList<Node> Destinations { get; } = new List<Node>();
            public IList<Node> Sources { get; } = new List<Node>();

            public int Height { get; }
            public bool Visited { get; set; }
            public bool End { get; }

            public int MinDistanceFromStart { get; set; } = NotDiscoveredDistance;

            public Node(int height, bool end = false)
            {
                Height = height;
                End = end;
            }

            public void AddDestinationIfReachable(Node destination)
            {
                if(destination.Height - Height <= 1)
                {
                    Destinations.Add(destination);
                    destination.Sources.Add(this);
                }
            }

            public void Reset()
            {
                Visited = false;
                MinDistanceFromStart = NotDiscoveredDistance;
            }
        }
    }
}

