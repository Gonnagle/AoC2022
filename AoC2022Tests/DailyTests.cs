using System.Reflection;
using AoC2022;
using FluentAssertions;
using Xunit.Abstractions;

namespace AoC2022Tests
{
    public class DailyTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Dictionary<int, Result> _expectedExampleResults;

        public DailyTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            _expectedExampleResults = new Dictionary<int, Result>()
            {
                {1, new Result("24000", "45000")},
                {2, new Result("15", "12")},
                {3, new Result("157", "70")},
                {4, new Result("2", "4")},
                {5, new Result("CMZ", "MCD")},
                {6, new Result("11", "26")},
                {7, new Result("95437", "24933642")},
                {8, new Result("21", "8")},
                {9, new Result("13", "1")},
                {10, new Result("13140", "1")}
            };
        }

        public record Result(string Part1, string Part2);

        public static IEnumerable<object[]> ExampleFiles()
            => TestUtilities.ExampleFiles();

        public static IEnumerable<object[]> InputFiles()
            => TestUtilities.InputFiles();

        [Theory]
        [MemberData(nameof(ExampleFiles))]
        public void ExamplePart1YieldsExpectedResult(string filePath, int day)
        {
            _testOutputHelper.WriteLine($"{day}, {filePath}");
            var lines = File.ReadLines(filePath);
            var methodInfo = ResolveMethodToTest(day, false);

            var result = methodInfo.Invoke(null, new object?[]{lines})!.ToString();
            _testOutputHelper.WriteLine(result);
            result.Should().Be(_expectedExampleResults[day].Part1);
        }

        [Fact]
        public void Day10Test(){
            var result = Day10.Part1(File.ReadLines("Inputs/Day10Example.txt"));
        }

        [Theory]
        [MemberData(nameof(InputFiles))]
        public void Part1Execution(string filePath, int day)
        {
            _testOutputHelper.WriteLine($"{day}, {filePath}");
            var lines = File.ReadLines(filePath);
            var methodInfo = ResolveMethodToTest(day, false);

            var result = methodInfo.Invoke(null, new object?[]{lines})!.ToString();
            Console.WriteLine($"Day {day} / Part 1 Result: {result}");
            _testOutputHelper.WriteLine(result);
        }

        [Theory]
        [MemberData(nameof(ExampleFiles))]
        public void ExamplePart2YieldsExpectedResult(string filePath, int day)
        {
            _testOutputHelper.WriteLine($"{day}, {filePath}");
            var lines = File.ReadLines(filePath);
            var methodInfo = ResolveMethodToTest(day, true);

            var result = methodInfo.Invoke(null, new object?[] { lines })!.ToString();

            _testOutputHelper.WriteLine(result);
            result.Should().Be(_expectedExampleResults[day].Part2);
        }

        [Theory]
        [MemberData(nameof(InputFiles))]
        public void Part2Execution(string filePath, int day)
        {
            _testOutputHelper.WriteLine($"{day}, {filePath}");
            var lines = File.ReadLines(filePath);
            var methodInfo = ResolveMethodToTest(day, true);

            var result = methodInfo.Invoke(null, new object?[] { lines })!.ToString();
            Console.WriteLine($"Day {day} / Part 2 Result: {result}");
            _testOutputHelper.WriteLine(result);
        }
        private static MethodInfo ResolveMethodToTest(int day, bool part2)
        {
            var className = $"Day{day}";
            var assembly = Assembly.GetAssembly(typeof(Day1));
            var type = assembly!.GetExportedTypes().First(x => x.Name == className);
            return part2 ? type.GetMethod(nameof(Day1.Part2))! : type.GetMethod(nameof(Day1.Part1))!;
        }
    }
}