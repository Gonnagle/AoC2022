namespace AoC2022
{
    public static class Day7
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var sumOfDirectoriesOverMaxSize = 0;

            _ = PopulateDirectoryTree(lines, ref sumOfDirectoriesOverMaxSize);

            return sumOfDirectoriesOverMaxSize.ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var sumNotNeeded = 0;
            var root = PopulateDirectoryTree(lines, ref sumNotNeeded);
            const int totalDiskSpace = 70000000;
            const int neededSpace = 30000000;

            var unusedSpace = totalDiskSpace - root.TotalSize;
            var missingSpace = neededSpace - unusedSpace;

            var candidates = FindDirectoriesOverGivenSize(root, new List<Directory>());

            List<Directory> FindDirectoriesOverGivenSize(Directory currentDirectory, List<Directory> candidates)
            {
                foreach (var subDirectory in currentDirectory.Subdirectories)
                {
                    candidates = FindDirectoriesOverGivenSize(subDirectory, candidates);
                }
                if (currentDirectory.TotalSize >= missingSpace)
                {
                    candidates.Add(currentDirectory);
                }
                return candidates;
            }

            return candidates.Min(x => x.TotalSize).ToString();
        }

        private static Directory PopulateDirectoryTree(IEnumerable<string> lines, ref int sumOfDirectoriesOverMaxSize)
        {
            var root = new Directory("/", null);
            var workingDirectory = root;
            const int maxSize = 100000;

            var localSumOfDirectoriesOverMaxSize = sumOfDirectoriesOverMaxSize;

            Directory? CountDirSizeAndTraverseUp()
            {
                workingDirectory!.CountSize();
                if (workingDirectory.TotalSize <= maxSize)
                {
                    localSumOfDirectoriesOverMaxSize += workingDirectory.TotalSize;
                }

                return workingDirectory.ParentDirectory;
            };

            foreach (var line in lines)
            {
                if (line.StartsWith("$ cd"))
                {
                    var targetDirectoryName = line.Split("cd ")[1];

                    if (targetDirectoryName == "..")
                    {
                        workingDirectory = CountDirSizeAndTraverseUp();
                    }
                    else if (targetDirectoryName != "/")
                    {
                        var newDir = new Directory(targetDirectoryName, workingDirectory);
                        workingDirectory!.Subdirectories.Add(newDir);
                        workingDirectory = newDir;
                    }
                }
                else if (!line.StartsWith("$"))
                {
                    var parts = line.Split(" ");
                    if (parts[0] != "dir")
                    {
                        workingDirectory!.Files.Add(new FileZ(parts[1], int.Parse(parts[0])));
                    }
                }
            }

            while (workingDirectory != null)
            {
                workingDirectory = CountDirSizeAndTraverseUp();
            }

            sumOfDirectoriesOverMaxSize = localSumOfDirectoriesOverMaxSize;

            return root;
        }
    }

    internal class Directory
    {
        public Directory(string name, Directory? parentDirectory = null)
        {
            Name = name;
            ParentDirectory = parentDirectory;
        }

        public string Name { get; }
        public Directory? ParentDirectory { get; }
        public int TotalSize { get; private set; } = 0;
        public IList<Directory> Subdirectories { get; } = new List<Directory>();
        public IList<FileZ> Files { get; } = new List<FileZ>();

        public void CountSize()
        {
            TotalSize = Subdirectories.Sum(x => x.TotalSize) + Files.Sum(x => x.Size);
        }
    }

    internal record FileZ(string Name, int Size);
}
