using Squares.Domain.Entities;

namespace Squares.Domain
{
    public static class SquareFinder
    {
        public static async Task<List<Square>> FindSquaresAsync(List<Point> points, CancellationToken cancellationToken)
        {
            var squares = new List<Square>();
            var uniquePoints = new HashSet<Point>(points);

            await Task.Run(() =>
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    for (int j = i + 1; j < points.Count; j++)
                    {
                        ProcessSquarePairs(points[i], points[j], uniquePoints, squares);
                    }
                }
            }, cancellationToken);

            return squares;
        }

        private static void ProcessSquarePairs(Point point1, Point point2, HashSet<Point> uniquePoints, List<Square> squares)
        {
            var possiblePairs = FindPointsPairs(point1, point2);

            foreach (var (item1, item2) in possiblePairs)
            {
                if (uniquePoints.Contains(item1) && uniquePoints.Contains(item2))
                {
                    var newSquare = new Square { Points = [point1, point2, item1, item2] };

                    if (!SquareExists(squares, newSquare))
                    {
                        squares.Add(newSquare);
                    }
                }
            }
        }

        private static bool SquareExists(List<Square> squares, Square newSquare)
        {
            return squares.Any(square => square.Points.All(p => newSquare.Points.Contains(p)));
        }

        private static List<(Point, Point)> FindPointsPairs(Point a, Point b)
        {
            var (deltaX, deltaY) = (b.X - a.X, b.Y - a.Y);

            var firstSet = CreatePointsPair(a, deltaY, -deltaX, b, deltaY, -deltaX);
            var secondSet = CreatePointsPair(a, -deltaY, deltaX, b, -deltaY, deltaX);

            return new List<(Point, Point)> { firstSet, secondSet };
        }

        private static (Point, Point) CreatePointsPair(Point basePoint, double xOffset1, double yOffset1, Point basePoint2, double xOffset2, double yOffset2)
        {
            var point1 = new Point(basePoint.X + xOffset1, basePoint.Y + yOffset1);
            var point2 = new Point(basePoint2.X + xOffset2, basePoint2.Y + yOffset2);

            return (point1, point2);
        }

        public static List<Square> UpdateSquaresAfterRemovingPoint(List<Square> squares, Point deletedPoint)
        {
            var newSquares = squares.Where(s => !s.Points.Contains(deletedPoint)).ToList();

            return newSquares;
        }
    }
}
