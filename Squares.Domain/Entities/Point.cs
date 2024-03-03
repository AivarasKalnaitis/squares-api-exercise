using Microsoft.EntityFrameworkCore;

namespace Squares.Domain.Entities
{
    [Owned]
    public class Point(double x, double y) : IComparable<Point>
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;

        public int CompareTo(Point other)
        {
            if (X.CompareTo(other.X) != 0)
            {
                return X.CompareTo(other.X);
            }
            return Y.CompareTo(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Point point)
                return false;

            return X == point.X && Y == point.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}