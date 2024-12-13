using System.Collections;
using TriangleLibrary;

namespace TriangleTests;

public class TriangleClassifierTests
{
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 2, 2)]
    [InlineData(8, 8, 8)]
    [InlineData(1.1, 1.1, 1.1)]
    public void ClassifyTriangle_OnEqualSides_ReturnsEquilateral(decimal side1, decimal side2, decimal side3)
    {
        var classifier = new TriangleClassifier();
        var expected = TriangleType.Equilateral;

        var actual = classifier.ClassifyTriangle(side1, side2, side3);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(2, 2, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(1.1, 2.2, 2.2)]
    public void ClassifyTriangle_OnTwoEqualSides_ReturnsIsosceles(decimal side1, decimal side2, decimal side3)
    {
        var classifier = new TriangleClassifier();
        var expected = TriangleType.Isosceles;

        var actual = classifier.ClassifyTriangle(side1, side2, side3);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(3, 4, 5)]
    [InlineData(5, 4, 3)]
    [InlineData(5, 3, 4)]
    [InlineData(3.1, 4.1, 5.1)]
    public void ClassifyTriangle_OnNoEqualSides_ReturnsScalene(decimal side1, decimal side2, decimal side3)
    {
        var classifier = new TriangleClassifier();
        var expected = TriangleType.Scalene;

        var actual = classifier.ClassifyTriangle(side1, side2, side3);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 1, 1, "side1")]
    [InlineData(1, 0, 1, "side2")]
    [InlineData(1, 1, 0, "side3")]
    [InlineData(-1, 1, 1, "side1")]
    [InlineData(1, -1, 1, "side2")]
    [InlineData(1, 1, -1, "side3")]
    [InlineData(0, 0, 0, "side1")]
    [InlineData(-1, -1, -1, "side1")]
    [InlineData(-1.1, 1.1, 1.1, "side1")]
    public void ClassifyTriangle_SideLessThanOrZero_ThrowsArgumentOutOfRangeException(decimal side1, decimal side2, decimal side3, string parameterName)
    {
        var classifier = new TriangleClassifier();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => classifier.ClassifyTriangle(side1, side2, side3));
        Assert.Equal(parameterName, exception.ParamName);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(3, 2, 1)]
    [InlineData(2, 3, 1)]
    [InlineData(5, 5, 11)]
    [InlineData(11, 5, 5)]
    [InlineData(5, 11, 5)]
    [InlineData(1.5, 1.5, 3)]
    public void ClassifyTriangle_InvalidSideLengths_ThrowsArgumentException(decimal side1, decimal side2, decimal side3)
    {
        var classifier = new TriangleClassifier();

        var exception = Assert.Throws<ArgumentException>(
            () => classifier.ClassifyTriangle(side1, side2, side3));
        Assert.StartsWith("Side lengths do not form a triangle", exception.Message);
    }

    private class OverflowValues : List<object[]>
    {
        public OverflowValues()
        {
            AddRange([
                [decimal.MaxValue, decimal.MaxValue, 1],
                [1, decimal.MaxValue, decimal.MaxValue],
                [decimal.MaxValue, 1, decimal.MaxValue],
            ]);
        }
    }

    [Theory]
    [ClassData(typeof(OverflowValues))]
    public void ClassifyTriangle_SideLengthsOverflow_ThrowsArgumentException(decimal side1, decimal side2, decimal side3)
    {
        var classifier = new TriangleClassifier();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => classifier.ClassifyTriangle(side1, side2, side3));
        Assert.StartsWith("Sides are too large", exception.Message);
    }
}
