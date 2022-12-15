using AoC2022;
using Xunit.Abstractions;
using FluentAssertions;

namespace AoC2022Tests
{
    public class CoordinateTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CoordinateTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("x: 12, y: 43", 12, 43)]
        [InlineData("(-2, 0)", -2, 0)]
        [InlineData("(3,5)", 3, 5)]
        [InlineData("The x coordinate is 23 and the y coordinate is -9", 23, -9)]
        [InlineData("Only order of the numbers matters: y is not 8, x != 293", 8, 293)]
        public void Coordinate_Initialized_WithStringWithTwoIntegers(string stringWithCoords, int x, int y)
        {
            var coordinate = new Coordinate(stringWithCoords);

            coordinate.X.Should().Be(x);
            coordinate.Y.Should().Be(y);
        }

        [Theory]
        [InlineData(12, 43)]
        [InlineData(-2, 0)]
        [InlineData(23, -9)]
        public void Coordinate_Initialized_WithIntegers(int x, int y)
        {
            var coordinate = new Coordinate(x, y);

            coordinate.X.Should().Be(x);
            coordinate.Y.Should().Be(y);
        }

        [Fact]
        public void Coordinate_EmptyConstructor_YieldsOrigo()
        {
            var coordinate = new Coordinate();

            coordinate.X.Should().Be(0);
            coordinate.Y.Should().Be(0);
        }

        [Fact]
        public void Coordinate_Deconstructs_IntoTwoIntegers()
        {
            var coordinate = Coordinate.Create(12, -34);

            var (deconstructedX, deconstructedY) = coordinate;
            deconstructedX.Should().Be(12);
            deconstructedY.Should().Be(-34);
        }

        [Fact]
        public void Coordinate_HasImplicitConversionFromValueTuple()
        {
            Coordinate coordinate = (7, -13);

            coordinate.X.Should().Be(7);
            coordinate.Y.Should().Be(-13);
        }

        [Fact]
        public void Coordinate_HasImplicitConversionToValueTuple()
        {
            (int, int) valueTuple = new Coordinate(5, 0);
            
            valueTuple.Item1.Should().Be(5);
            valueTuple.Item2.Should().Be(0);
        }

        [Fact]
        public void Coordinate_ToString_YieldsCoordinateNotation()
        {
            var coordinate = new Coordinate(9, -3);

            coordinate.ToString().Should().Be("(9,-3)");
        }

        [Theory]
        [InlineData("(1,1)", "(2,3)", 3, 4)]
        [InlineData("(1,1)", "(-2,-3)", -1, -2)]
        [InlineData("(5,2)", "(0,0)", 5, 2)]
        public void Coordinate_Addition_YieldsPairwiseSum(string a, string b, int resultX, int resultY)
        {
            var coordinateA = new Coordinate(a);
            var coordinateB = new Coordinate(b);
            var result = coordinateA + coordinateB;

            result.X.Should().Be(resultX);
            result.Y.Should().Be(resultY);
        }

        [Theory]
        [InlineData("(1,1)", "(2,3)", false)]
        [InlineData("(0,1)", "(0,-3)", false)]
        [InlineData("(1,2)", "(0,2)", false)]
        [InlineData("(2,-3)", "(2,-3)", true)]
        public void Coordinate_SameCoordinate_YieldsExpectedResults(string a, string b, bool same)
        {
            var coordinateA = new Coordinate(a);
            var coordinateB = new Coordinate(b);
            var result = coordinateA.SameCoordinate(coordinateB);

            result.Should().Be(same);
        }

        [Theory]
        [InlineData("(1,1)", "(2,3)", 3)]
        [InlineData("(0,1)", "(0,-3)", 4)]
        [InlineData("(1,2)", "(0,2)", 1)]
        [InlineData("(2,-3)", "(2,-3)", 0)]
        public void Coordinate_StaticManhattanDistance(string a, string b, int distance)
        {
            var coordinateA = new Coordinate(a);
            var coordinateB = new Coordinate(b);
            var result = Coordinate.ManhattanDistance(coordinateA, coordinateB);

            result.Should().Be(distance);
        }


        [Theory]
        [InlineData("(1,1)", "(2,3)", 3)]
        [InlineData("(0,1)", "(0,-3)", 4)]
        [InlineData("(1,2)", "(0,2)", 1)]
        [InlineData("(2,-3)", "(2,-3)", 0)]
        public void Coordinate_ManhattanDistance(string a, string b, int distance)
        {
            var coordinateA = new Coordinate(a);
            var coordinateB = new Coordinate(b);
            var result = coordinateA.ManhattanDistance(coordinateB);

            result.Should().Be(distance);
        }
    }
}