using System.Text.RegularExpressions;

namespace AoC2022Tests
{
    internal static class TestUtilities
    {
        public static IEnumerable<TestFile> TestFiles()
        {
            var files = Directory.EnumerateFiles(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Inputs"));
            foreach (var file in files)
            {
                yield return new TestFile(Path.GetRelativePath(Directory.GetCurrentDirectory(), file), ResolveDayFromFileName(Path.GetFileName(file)),
                    file.Contains("Example", StringComparison.InvariantCultureIgnoreCase)
                );
            }
        }

        public static IEnumerable<object[]> ExampleFiles() => 
            TestFiles().Where(x => x.Example).OrderBy(x => x.Day).Select(x => new object[] {x.Path, x.Day});

        public static IEnumerable<object[]> InputFiles() =>
            TestFiles().Where(x => !x.Example).OrderBy(x => x.Day).Select(x => new object[] { x.Path, x.Day });


        public record TestFile(string Path, int Day, bool Example);

        public static int ResolveDayFromFileName(string fileName)
        {
            const string dayNumberPattern = @"^Day(\d+)";
            var dayNumberRegex = new Regex(dayNumberPattern, RegexOptions.IgnoreCase);
            var match = dayNumberRegex.Match(fileName);
            var className = match.Groups[1].Value;
            return int.Parse(className);
        }
    }
}
