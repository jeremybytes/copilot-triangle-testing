namespace TriangleLibrary
{
    public enum TriangleType
    {
        Equilateral,
        Isosceles,
        Scalene
    }

    public class TriangleClassifier
    {
        public TriangleType ClassifyTriangle(double side1, double side2, double side3)
        {
            if (side1 <= 0 || side2 <= 0 || side3 <= 0)
            {
                throw new ArgumentOutOfRangeException("Sides must be greater than zero.");
            }

            if (double.IsInfinity(side1) || double.IsInfinity(side2) || double.IsInfinity(side3) ||
                double.IsNaN(side1) || double.IsNaN(side2) || double.IsNaN(side3))
            {
                throw new ArgumentOutOfRangeException("Sides must be finite numbers.");
            }

            // Check for potential overflow in addition
            if (side1 > double.MaxValue - side2 || side1 > double.MaxValue - side3 || side2 > double.MaxValue - side3)
            {
                throw new ArgumentOutOfRangeException("Sides are too large.");
            }

            if (side1 + side2 <= side3 || side1 + side3 <= side2 || side2 + side3 <= side1)
            {
                throw new ArgumentException("The given sides do not form a valid triangle.");
            }

            if (side1 == side2 && side2 == side3)
            {
                return TriangleType.Equilateral;
            }
            else if (side1 == side2 || side2 == side3 || side1 == side3)
            {
                return TriangleType.Isosceles;
            }
            else
            {
                return TriangleType.Scalene;
            }
        }
    }
}
