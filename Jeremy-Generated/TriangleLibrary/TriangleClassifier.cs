namespace TriangleLibrary;

public enum TriangleType
{
    Equilateral,
    Isosceles,
    Scalene,
}

public class TriangleClassifier
{
    public TriangleType ClassifyTriangle(decimal side1, decimal side2, decimal side3)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(side1, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(side2, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(side3, 0);

        try
        {
            if ((side1 + side2 <= side3) || (side2 + side3 <= side1) || (side1 + side3 <= side2))
            {
                throw new ArgumentException("Side lengths do not form a triangle");
            }
        }
        catch (OverflowException)
        {
            throw new ArgumentOutOfRangeException("unspecified", "Sides are too large");
        }

        return (side1, side2, side3) switch
        {
            _ when (side1 == side2) && (side1 == side3) => TriangleType.Equilateral,
            _ when side1 == side2 => TriangleType.Isosceles,
            _ when side2 == side3 => TriangleType.Isosceles,
            _ when side1 == side3 => TriangleType.Isosceles,
            _ => TriangleType.Scalene,
        };
    }
}
