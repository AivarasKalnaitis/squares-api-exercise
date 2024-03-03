namespace Squares.Domain.Entities
{
    public class PointsList
    {
        public int Id { get; set; }
        public List<Point> Points { get; set; }
        public List<Square> Squares { get; set; }
        public bool ShouldUpdateSquares { get; set; }

        public PointsList()
        {
            ShouldUpdateSquares = true;
        }
    }
}