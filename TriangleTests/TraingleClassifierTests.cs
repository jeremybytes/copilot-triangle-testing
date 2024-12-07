using TriangleLibrary;
using Xunit;

namespace TriangleTests
{
    public class TriangleClassifierTests
    {
        [Theory]
        [InlineData(3, 3, 3)]
        [InlineData(5, 5, 5)]
        [InlineData(10, 10, 10)]
        [InlineData(3.3, 3.3, 3.3)] // Non-integer values
        public void ClassifyTriangle_Equilateral(double side1, double side2, double side3)
        {
            // Arrange
            var classifier = new TriangleClassifier();

            // Act
            var result = classifier.ClassifyTriangle(side1, side2, side3);

            // Assert
            Assert.Equal(TriangleType.Equilateral, result);
        }

        [Theory]
        [InlineData(3, 3, 2)]
        [InlineData(5, 5, 3)]
        [InlineData(10, 10, 5)]
        [InlineData(2, 2, 3)] // Edge case: smallest isosceles triangle
        [InlineData(3.3, 3.3, 2.2)] // Non-integer values
        public void ClassifyTriangle_Isosceles(double side1, double side2, double side3)
        {
            // Arrange
            var classifier = new TriangleClassifier();

            // Act
            var result = classifier.ClassifyTriangle(side1, side2, side3);

            // Assert
            Assert.Equal(TriangleType.Isosceles, result);
        }

        [Theory]
        [InlineData(3, 4, 5)]
        [InlineData(5, 6, 7)]
        [InlineData(7, 8, 9)]
        [InlineData(2, 3, 4)] // Edge case: smallest scalene triangle
        [InlineData(3.1, 4.1, 5.1)] // Non-integer values
        public void ClassifyTriangle_Scalene(double side1, double side2, double side3)
        {
            // Arrange
            var classifier = new TriangleClassifier();

            // Act
            var result = classifier.ClassifyTriangle(side1, side2, side3);

            // Assert
            Assert.Equal(TriangleType.Scalene, result);
        }

        [Theory]
        [InlineData(0, 3, 3)]
        [InlineData(3, 0, 3)]
        [InlineData(3, 3, 0)]
        [InlineData(-1, 3, 3)]
        [InlineData(3, -1, 3)]
        [InlineData(3, 3, -1)]
        [InlineData(-1, -1, -1)] // Edge case: all sides negative
        [InlineData(0.0, 3.3, 3.3)] // Non-integer values
        [InlineData(3.3, 0.0, 3.3)] // Non-integer values
        [InlineData(3.3, 3.3, 0.0)] // Non-integer values
        [InlineData(-1.1, 3.3, 3.3)] // Non-integer values
        [InlineData(3.3, -1.1, 3.3)] // Non-integer values
        [InlineData(3.3, 3.3, -1.1)] // Non-integer values
        public void ClassifyTriangle_ZeroOrNegativeSides_ThrowsArgumentOutOfRangeException(double side1, double side2, double side3)
        {
            // Arrange
            var classifier = new TriangleClassifier();

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => classifier.ClassifyTriangle(side1, side2, side3));
            Assert.Equal("Sides must be greater than zero.", exception.ParamName);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 10, 12)]
        [InlineData(5, 1, 1)]
        [InlineData(2, 2, 5)]
        [InlineData(1, 1, 2)] // Edge case: sum of two sides equals the third side
        [InlineData(1.5, 1.5, 3)] // Edge case: sum of two sides equals the third side with floating point
        [InlineData(1.1, 2.2, 3.3)] // Non-integer values
        [InlineData(1.1, 10.1, 12.2)] // Non-integer values
        [InlineData(5.5, 1.1, 1.1)] // Non-integer values
        [InlineData(2.2, 2.2, 5.5)] // Non-integer values
        public void ClassifyTriangle_InvalidTriangles_ThrowsArgumentException(double side1, double side2, double side3)
        {
            // Arrange
            var classifier = new TriangleClassifier();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => classifier.ClassifyTriangle(side1, side2, side3));
            Assert.Equal("The given sides do not form a valid triangle.", exception.Message);
        }

        [Theory]
        [InlineData(double.MaxValue / 2, double.MaxValue / 2, double.MaxValue / 2)] // Edge case: large double values
        [InlineData(double.MaxValue / 2, double.MaxValue / 2, 1)] // Edge case: large double values with a small side
        [InlineData(double.MaxValue / 2, 1, 1)] // Edge case: large double value with two small sides
        [InlineData(double.MinValue, double.MinValue, double.MinValue)] // Edge case: minimum double values
        [InlineData(double.MinValue, double.MinValue, -1)] // Edge case: minimum double values with a small negative side
        [InlineData(double.MinValue, -1, -1)] // Edge case: minimum double value with two small negative sides
        public void ClassifyTriangle_OverflowValues_ThrowsArgumentOutOfRangeException(double side1, double side2, double side3)
        {
            // Arrange
            var classifier = new TriangleClassifier();

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => classifier.ClassifyTriangle(side1, side2, side3));
            Assert.Equal("Sides are too large.", exception.ParamName);
        }
    }
}
